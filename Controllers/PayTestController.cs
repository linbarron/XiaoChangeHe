using Senparc.Weixin;
using Senparc.Weixin.BrowserUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using ZXing;
using ZXing.Common;

namespace WitBird.XiaoChangHe.Controllers
{
    public class PayTestController : Controller
    {

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        private static TenPayV3Info _tenPayV3Info;
        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }

        /// <summary>
        /// 获取用户的OpenId,m=c,订单消费；m=r:用户充值, n=用户名，t=电话，a=充值金额
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public ActionResult Index(int id, int hc, string uid, string m, string n, string t, string a)
        {
            if (!"c".Equals(m, StringComparison.CurrentCultureIgnoreCase) || !"r".Equals(m, StringComparison.CurrentCultureIgnoreCase))
            {
                return Content("非正常交易请求");
            }
            var returnUrl = string.Format("http://test.xgdg.cn/paytest/JsApi");
            var state = string.Format("{0}|{1}|{2}|{3}", id, hc, uid, m);
            var url = OAuthApi.GetAuthorizeUrl(TenPayV3Info.AppId, returnUrl, state, OAuthScope.snsapi_userinfo);

            return Redirect(url);
        }

        #region JsApi支付

        public ActionResult JsApi(string code, string state)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Content("您拒绝了授权！");
                }

                if (!state.Contains("|"))
                {
                    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                    return Content("验证失败！请从正规途径进入！1001");
                }

                //获取产品信息
                var stateData = state.Split('|');
                string orderId = stateData[0];
                string uid = stateData[2];
                string orderType = stateData[3];

                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";


                //通过，用code换取access_token
                var openIdResult = OAuthApi.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    return Content("错误：" + openIdResult.errmsg);
                }

                PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                CrmMemberModel crmMemberModel = new CrmMemberModel();
                BillPayModel billPayModel = new BillPayModel();

                Models.Info.PrepayRecord prepayRecord = null;
                Models.Info.PrepayAccount prepayAccount = null;
                Models.Info.BillPay billPay = null;

                decimal totalPrice = 0;

                #region 消费
                if (orderType.Equals("c", StringComparison.CurrentCultureIgnoreCase))
                {
                    Models.Info.Order order = null;
                    List<Models.Info.OrderDetails> orderDetails = null;

                    OrderModel orderModel = new OrderModel();
                    OrderDetailsModel orderDetaisModel = new OrderDetailsModel();

                    int hc = 0;
                    if (int.TryParse(stateData[1], out hc))
                    {
                        order = orderModel.selOrderId(orderId).FirstOrDefault();

                        if (order == null || order.GetHashCode() != hc)
                        {
                            return Content("商品信息不存在，或非法进入！1002");
                        }

                        ViewBag["Order"] = order;
                    }


                    orderDetails = orderDetaisModel.getOrderDetaiById(orderId);
                    if (orderDetails.Count > 0)
                    {
                        for (int i = 0; i < orderDetails.Count; i++)
                        {
                            if (orderDetails[i].UnitPrice != 0)
                            {
                                totalPrice += orderDetails[i].UnitPrice * orderDetails[i].ProductCount;
                            }
                        }
                    }

                    prepayAccount = crmMemberModel.GetPrepayAccount(uid);

                    prepayRecord = new PrepayRecord();
                    prepayRecord.AddMoney = -totalPrice;
                    prepayRecord.AsureDate = DateTime.Now;
                    prepayRecord.BillPayId = Guid.NewGuid();
                    prepayRecord.DiscountlMoeny = 0;
                    prepayRecord.PayByScore = 0;
                    prepayRecord.PayModel = "02";
                    prepayRecord.PrepayDate = DateTime.Now;
                    prepayRecord.PrepayMoney = totalPrice;
                    prepayRecord.PresentMoney = prepayAccount.PresentMoney;
                    prepayRecord.PromotionId = 0;
                    prepayRecord.RecMoney = 0;
                    prepayRecord.RecordId = -1;
                    prepayRecord.RState = "";
                    prepayRecord.RstId = Guid.Parse("CB824E58-E2CA-4C95-827A-CA62D528C6A7");
                    prepayRecord.ScoreVip = 0;
                    prepayRecord.SId = orderId;
                    prepayRecord.Uid = uid;
                    prepayRecord.UserId = "System";

                    decimal creditCard = totalPrice - (prepayAccount.AccountMoney + prepayAccount.PresentMoney);
                    if (creditCard < 0) creditCard = 0;

                    billPay = new BillPay();
                    billPay.Cash = 0;
                    billPay.Change = 0;
                    billPay.Coupons = 0;
                    billPay.CouponsNo = "";
                    billPay.CreateDate = DateTime.Now;
                    billPay.CreditCard = totalPrice;
                    billPay.Discount = 0;
                    billPay.MemberCard = 0;
                    billPay.MemberCardNo = "";
                    billPay.PaidIn = totalPrice;
                    billPay.PayId = prepayRecord.BillPayId.Value;
                    billPay.PayState = BillPayState.NotPaid;
                    billPay.Receivable = totalPrice;
                    billPay.Remark = "支付订单：" + orderId;
                    billPay.Remove = 0;
                    billPay.RstId = Guid.Parse("CB824E58-E2CA-4C95-827A-CA62D528C6A7");
                    billPay.UserId = Guid.Empty;
                    billPay.UserName = order.ContactName;

                    if (!prepayRecordModel.AddPrepayRecord(prepayRecord))
                    {
                        return Content("交易错误");
                    }

                    if (!billPayModel.AddBillPay(billPay))
                    {
                        return Content("交易错误");
                    }

                }
                #endregion 消费

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();

                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();

                //设置package订单参数
                packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
                packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                packageReqHandler.SetParameter("body", "订单支付");    //商品信息
                packageReqHandler.SetParameter("out_trade_no", billPay.PayId.ToString());		//商家订单号
                packageReqHandler.SetParameter("total_fee", (Convert.ToInt32(billPay.CreditCard * 100)).ToString());			        //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", TenPayV3Info.TenPayV3Notify);		    //接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());	                    //交易类型
                packageReqHandler.SetParameter("openid", openIdResult.openid);	                    //用户的openId

                string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                packageReqHandler.SetParameter("sign", sign);	                    //签名

                string data = packageReqHandler.ParseXML();

                var result = TenPayV3.Unifiedorder(data);
                var res = XDocument.Parse(result);
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", TenPayV3Info.AppId);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);

                ViewData["appId"] = TenPayV3Info.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
            }
            catch (Exception ex)
            {
            }

            return View();
        }

        public ActionResult RechargeNotifyUrl()
        {
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;

            try
            {

                resHandler.SetKey(TenPayV3Info.Key);
                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign())
                {
                    res = "success";

                    //正确的订单处理

                    string out_trade_no = resHandler.GetParameter("out_trade_no");
                    string total_fee = resHandler.GetParameter("total_fee");
                    //微信支付订单号
                    string transaction_id = resHandler.GetParameter("transaction_id");
                    //支付完成时间
                    string time_end = resHandler.GetParameter("time_end");

                    BillPay billPay = null;
                    BillPayModel billPayModel = new BillPayModel();

                    Guid billPayId = Guid.Parse(out_trade_no);
                    billPay = billPayModel.GetBillPayById(billPayId);

                    if (billPay == null)
                    {
                        res = "订单:" + billPayId + "不存在";
                        return_msg = res;
                        return_code = "FAIL";
                    }
                    else if (billPay.CreditCard != (decimal)(Convert.ToDecimal(total_fee)/100))
                    {
                        res = "订单金额不符合";
                        return_msg = res;
                        return_code = "FAIL";
                    }
                    else if (!billPay.PayState.Equals(BillPayState.Paid)) //没有处理过该订单
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            //更新订单状态为已支付，记录交易流水号
                            if (billPayModel.UpdateBillStateAsPaid(billPayId, transaction_id))
                            {
                                
                                CrmMemberModel crmMemberModel = new CrmMemberModel();
                                PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                                //查询支付订单对应的充值记录
                                PrepayRecord prepayRecord = prepayRecordModel.GetPrepayRecordByBillPayId(billPayId);

                                string uid = prepayRecord.Uid;
                                //查询个人余额账户
                                PrepayAccount prepayAccount = crmMemberModel.GetPrepayAccount(prepayRecord.Uid);
                                //更新个人账户                                
                                prepayAccount.LastPresentMoney = prepayRecord.PresentMoney;
                                prepayAccount.AccountMoney += prepayRecord.PrepayMoney.Value;
                                prepayAccount.PresentMoney += prepayRecord.PresentMoney.Value;
                                prepayAccount.TotalPresent = prepayAccount.PresentMoney;
                                prepayAccount.TotalMoney = prepayAccount.AccountMoney + prepayAccount.PresentMoney;

                                //更新支付时间
                                //prepayRecord.AsureDate = DateTime.Now;

                                if (crmMemberModel.UpdatePrepayAccount(prepayAccount))
                                {
                                    return_code = "SUCCESS";
                                    return_msg = "OK";
                                }
                            }

                            scope.Complete();
                        }
                    }
                }
                else
                {
                    res = "非法支付结果通知";

                    //错误的订单处理
                }

            }
            catch (Exception ex)
            {
                res += ex.ToString();
                return_code = "FAIL";
                return_msg = ex.ToString();
            }

            var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/1.txt"));
            fileStream.Write(Encoding.Default.GetBytes(res), 0, Encoding.Default.GetByteCount(res));
            fileStream.Close();
            string xml = string.Format(@"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);

            return Content(xml, "text/xml");
        }

        #endregion

        #region 订单及退款

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderQuery()
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("transaction_id", "");       //填入微信订单号 
            packageReqHandler.SetParameter("out_trade_no", "");         //填入商家订单号
            packageReqHandler.SetParameter("nonce_str", nonceStr);             //随机字符串
            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.OrderQuery(data);
            var res = XDocument.Parse(result);
            string openid = res.Element("xml").Element("sign").Value;

            return Content(openid);
        }

        /// <summary>
        /// 关闭订单接口
        /// </summary>
        /// <returns></returns>
        public ActionResult CloseOrder()
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("out_trade_no", "");                 //填入商家订单号
            packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.CloseOrder(data);
            var res = XDocument.Parse(result);
            string openid = res.Element("xml").Element("openid").Value;

            return Content(openid);
        }

        /// <summary>
        /// 退款申请接口
        /// </summary>
        /// <returns></returns>
        public ActionResult Refund()
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("out_trade_no", "");                 //填入商家订单号
            packageReqHandler.SetParameter("out_refund_no", "");                //填入退款订单号
            packageReqHandler.SetParameter("total_fee", "");               //填入总金额
            packageReqHandler.SetParameter("refund_fee", "");               //填入退款金额
            packageReqHandler.SetParameter("op_user_id", TenPayV3Info.MchId);   //操作员Id，默认就是商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名
            //退款需要post的数据
            string data = packageReqHandler.ParseXML();

            //退款接口地址
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
            string cert = @"F:\apiclient_cert.p12";
            //私钥（在安装证书时设置）
            string password = "";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //调用证书
            X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            #region 发起post请求
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";

            byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();
            #endregion

            var res = XDocument.Parse(responseContent);
            string openid = res.Element("xml").Element("out_refund_no").Value;

            return Content(openid);
        }
        #endregion

        #region 红包

        /// <summary>
        /// 目前支持向指定微信用户的openid发放指定金额红包
        /// 注意total_amount、min_value、max_value值相同
        /// total_num=1固定
        /// 单个红包金额介于[1.00元，200.00元]之间
        /// </summary>
        /// <returns></returns>
        public ActionResult SendRedPack()
        {
            string mchbillno = DateTime.Now.ToString("HHmmss") + TenPayV3Util.BuildRandomStr(28);

            string nonceStr = TenPayV3Util.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
            packageReqHandler.SetParameter("wxappid", TenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("mch_billno", mchbillno);                 //填入商家订单号
            packageReqHandler.SetParameter("nick_name", "提供方名称");                 //提供方名称
            packageReqHandler.SetParameter("send_name", "红包发送者名称");                 //红包发送者名称
            packageReqHandler.SetParameter("re_openid", "接受收红包的用户的openId");                 //接受收红包的用户的openId
            packageReqHandler.SetParameter("total_amount", "100");                //付款金额，单位分
            packageReqHandler.SetParameter("min_value", "100");                //最小红包金额，单位分
            packageReqHandler.SetParameter("max_value", "100");                //最大红包金额，单位分
            packageReqHandler.SetParameter("total_num", "1");               //红包发放总人数
            packageReqHandler.SetParameter("wishing", "红包祝福语");               //红包祝福语
            packageReqHandler.SetParameter("client_ip", Request.UserHostAddress);               //调用接口的机器Ip地址
            packageReqHandler.SetParameter("act_name", "活动名称");   //活动名称
            packageReqHandler.SetParameter("remark", "备注信息");   //备注信息
            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名
            //发红包需要post的数据
            string data = packageReqHandler.ParseXML();

            //发红包接口地址
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
            string cert = @"F:\apiclient_cert.p12";
            //私钥（在安装证书时设置）
            string password = "";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //调用证书
            X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            #region 发起post请求
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";

            byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();
            #endregion

            return Content(responseContent);
        }
        #endregion
    }
}
