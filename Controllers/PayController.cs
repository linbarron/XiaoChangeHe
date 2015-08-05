using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Xml.Linq;
using WitBird.XiaoChangeHe.Core;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe.Models.Specials;

namespace WitBird.XiaoChangHe.Controllers
{
    public class PayController : Controller
    {
        #region Recharge

        private static TenPayV3Info _tenPayV3Info;
        /// <summary>
        /// 微信支付相关配置信息
        /// </summary>
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
        /// 充值前准备参数方法，微信登录验证
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult PreRecharge(string id, string name)
        {
            var returnUrl = Constants.HostDomain + "/pay/recharge?name=" + name + "&showwxpaytitle=1";
            var state = "";
            var url = OAuthApi.GetAuthorizeUrl(TenPayV3Info.AppId, returnUrl, state, OAuthScope.snsapi_userinfo);

            return Redirect(url);
        }

        /// <summary>
        /// Handles user recharging request.
        /// </summary>
        /// <param name="id">Commpany id.</param>
        /// <param name="name">SourceAccountId, corresponding to wechat unique name.</param>
        /// <returns></returns>
        public ActionResult Recharge(string code, string state, string name)
        {
            CrmMember crmMember = null;

            try
            {
                //string name = Session["SourceAccountId"].ToString();
                if (!string.IsNullOrEmpty(name))
                {
                    CrmMemberModel cdb = new CrmMemberModel();
                    crmMember = cdb.getCrmMemberListInfoData(name).FirstOrDefault();

                    if (crmMember != null)
                    {
                        PrepayRecordModel prm = new PrepayRecordModel();
                        var rechargeRecords = prm.getRechargeRecordListInfoData(crmMember.Uid);
                        if (rechargeRecords != null && rechargeRecords.Count > 0)
                        {
                            ViewBag.IsFirstRecharge = false;
                        }
                        else
                        {
                            ViewBag.IsFirstRecharge = true;
                        }

                        ViewBag.Code = code;
                        ViewBag.State = state;
                        ViewBag.Uid = crmMember.Uid;
                        ViewBag.SourceAccountId = name;
                        ViewBag.CompanyId = Constants.CompanyId;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.WxPay, ex);
            }
            finally
            {
            }

            return View(crmMember);
        }

        /// <summary>
        /// Handles user recharging request.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="tel"></param>
        /// <param name="amount"></param>
        /// <param name="isFirstRecharge"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Recharge(string code, string uid, string name, string tel, string amount, bool isFirstRecharge)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    throw new ArgumentException("您拒绝了授权");
                }

                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";


                //通过，用code换取access_token
                var openIdResult = OAuthApi.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    throw new ArgumentException("微信AccessToken错误，" + openIdResult.errcode);
                }

                PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                CrmMemberModel crmMemberModel = new CrmMemberModel();
                OrderBillPayModel billPayModel = new OrderBillPayModel();

                Models.Info.PrepayRecord prepayRecord = null;
                Models.Info.OrderBillPay billPay = null;

                decimal totalPrice = 0;
                decimal presentMoney = 0;

                if (!decimal.TryParse(amount, out totalPrice))
                {
                    throw new ArgumentException("充值金额错误，" + amount);
                }

                if (isFirstRecharge && totalPrice != 1000)
                {
                    throw new ArgumentException("充值金额错误，" + amount);
                }

                if (!isFirstRecharge && totalPrice != 500 && totalPrice != 1000)
                {
                    throw new ArgumentException("充值金额错误，" + amount);
                }

                //强制设置为配置的值。
                //totalPrice = 0.01m;

                if (isFirstRecharge)
                {
                    presentMoney = 300;
                }
                else
                {
                    presentMoney = (totalPrice / 500) * 100;
                }

                prepayRecord = new PrepayRecord();
                prepayRecord.AddMoney = totalPrice;
                prepayRecord.AsureDate = DateTime.Now;
                prepayRecord.BillPayId = Guid.NewGuid();
                prepayRecord.DiscountlMoeny = 0;
                prepayRecord.PayByScore = 0;
                prepayRecord.PayModel = "02";
                prepayRecord.PrepayDate = DateTime.Now;
                prepayRecord.PrepayMoney = totalPrice;
                prepayRecord.PresentMoney = presentMoney;
                prepayRecord.PromotionId = 0;
                prepayRecord.RecMoney = 0;
                prepayRecord.RecordId = -1;
                prepayRecord.RState = "00";
                prepayRecord.RstId = Constants.CompanyId;
                prepayRecord.ScoreVip = 0;
                prepayRecord.SId = Guid.NewGuid().ToString();//DateTime.Now.ToString("HHmmss") + TenPayV3Util.BuildRandomStr(28);
                prepayRecord.Uid = uid;
                prepayRecord.UserId = "System";

                billPay = new OrderBillPay();
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
                billPay.Remark = "用户充值" + totalPrice + "元";
                billPay.Remove = 0;
                billPay.RstId = Constants.CompanyId;
                billPay.UserId = Guid.Empty;
                billPay.UserName = name;

                if (!prepayRecordModel.AddPrepayRecord(prepayRecord))
                {
                    throw new Exception("消费记录插入失败");
                }

                if (!billPayModel.AddBillPay(billPay))
                {
                    throw new Exception("交易订单数据记录插入失败");
                }


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
                packageReqHandler.SetParameter("body", billPay.Remark);    //商品信息
                packageReqHandler.SetParameter("out_trade_no", billPay.PayId.ToString("N"));		//商家订单号
                packageReqHandler.SetParameter("total_fee", (Convert.ToInt32(billPay.CreditCard * 100)).ToString());			        //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", ConfigurationManager.AppSettings["TenPayV3_TenpayNotify"]);		    //接收财付通通知的URL
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
                Logger.Log("调用订单支付：" + ex.ToString());
                var failedData = new { IsSuccess = false, Message = "支付请求失败，请重新尝试。如多次遇到此问题，请联系客服" };
                return Json(failedData, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = new
            {
                IsSuccess = true,
                Message = "",
                appId = ViewData["appId"],
                timeStamp = ViewData["timeStamp"],
                nonceStr = ViewData["nonceStr"],
                package = ViewData["package"],
                paySign = ViewData["paySign"]
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户微信充值成功异步回调方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RechargePayNotifyUrl()
        {
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");
            return_code = "FAIL";
            return_msg = "处理支付通知出现异常";
            string res = null;

            try
            {

                resHandler.SetKey(TenPayV3Info.Key);
                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign())
                {
                    //正确的订单处理

                    string out_trade_no = resHandler.GetParameter("out_trade_no");
                    string total_fee = resHandler.GetParameter("total_fee");
                    //微信支付订单号
                    string transaction_id = resHandler.GetParameter("transaction_id");
                    //支付完成时间
                    string time_end = resHandler.GetParameter("time_end");

                    OrderBillPay billPay = null;
                    OrderBillPayModel billPayModel = new OrderBillPayModel();

                    Guid billPayId = Guid.Parse(out_trade_no);
                    billPay = billPayModel.GetBillPayById(billPayId);

                    if (billPay == null)
                    {
                        res = "订单:" + billPayId + "不存在";
                        return_msg = res;
                    }
                    else if (billPay.CreditCard != (decimal)(Convert.ToDecimal(total_fee) / 100))
                    {
                        res = "订单金额不符合";
                        return_msg = res;
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

                                if (prepayRecord == null)
                                {
                                    res = "查询不到对应的消费记录";
                                    return_msg = res;
                                }
                                else
                                {
                                    string uid = prepayRecord.Uid;
                                    //查询个人余额账户
                                    PrepayAccount prepayAccount = crmMemberModel.GetPrepayAccount(prepayRecord.Uid);

                                    if (prepayAccount == null)
                                    {
                                        res = "查询不到用户账户记录";
                                        return_msg = res;
                                    }
                                    else
                                    {
                                        //更新个人账户                                
                                        prepayAccount.LastPresentMoney = prepayRecord.PresentMoney;
                                        prepayAccount.AccountMoney += prepayRecord.PrepayMoney.Value;
                                        prepayAccount.PresentMoney += prepayRecord.PresentMoney.Value;
                                        prepayAccount.TotalPresent = prepayAccount.PresentMoney;
                                        prepayAccount.TotalMoney = prepayAccount.AccountMoney + prepayAccount.PresentMoney;

                                        //更新支付时间及状态
                                        prepayRecord.AsureDate = DateTime.Now;
                                        prepayRecord.RState = "01";

                                        if (crmMemberModel.UpdatePrepayAccount(prepayAccount) && 
                                            prepayRecordModel.UpdatePrepayRecord(prepayRecord))
                                        {
                                            //提交事务
                                            scope.Complete();
                                            return_code = "SUCCESS";
                                            return_msg = "OK";
                                        }
                                        else
                                        {
                                            res = "更新用户账户信息未成功";
                                            return_msg = res;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        res = "订单：" + billPay.PayId + "已处理，不能重复处理";
                        return_code = "SUCCESS";
                        return_msg = "OK";
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

            string xml = string.Format(@"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);

            Logger.Log(LoggingLevel.WxPay, res);
            Logger.Log(LoggingLevel.WxPay, xml);
            return Content(xml, "text/xml");
        }

        #endregion Recharge

        #region Order
        /// <summary>
        /// 我的订单确认支付页面
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="MemberCardNo"></param>
        /// <param name="OrderId"></param>
        /// <param name="SourceAccountId"></param>
        /// <param name="CompanyId"></param>
        /// <param name="Type"></param>
        /// <param name="RstType"></param>
        /// <returns></returns>
        public ActionResult MyOrderDetail(string code, string state, string MemberCardNo, string OrderId, string SourceAccountId,
            string CompanyId = null, string Type = null, string RstType = null)
        {
            try
            {
                ViewBag.Code = code;
                ViewBag.State = state;
                ViewBag.Uid = MemberCardNo;
                ViewBag.CompanyId = Constants.CompanyId;
                ViewBag.RstType = RstType;
                ViewBag.SourceAccountId = SourceAccountId;
                ViewBag.OrderId = OrderId;
                ViewBag.MemberCardNo = MemberCardNo;

                MyMenuModel odb = new MyMenuModel();
                CrmMemberModel cdb1 = new CrmMemberModel();

                PrepayAccount prepayAccount = cdb1.GetPrepayAccount(MemberCardNo);
                ViewBag.PrepayAccount = prepayAccount.AccountMoney;
                ViewBag.TotalAmount = prepayAccount.AccountMoney + prepayAccount.PresentMoney;

                string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
                ViewBag.RestaurantId = RestaurantId;

                var order = new OrderModel().selOrderByOrderId(Guid.Parse(OrderId)).FirstOrDefault();
                List<MyOrderDetail> detail = odb.getMyOrderDetailListData(MemberCardNo, OrderId, RstType);
                // ViewBag.MyOrderDetail = detail;

                decimal totalPrice = 0;
                decimal totalVipPrice = 0;
                if (order != null && detail.Count > 0)
                {
                    var allTodaySpecials = SpecialsModel.GetTodayByRestaurantId(order.RstId);

                    for (int i = 0; i < detail.Count; i++)
                    {
                        if (allTodaySpecials.Any(x => x.ProductId == detail[i].proId))
                        {
                            var special = allTodaySpecials.Where(x => x.ProductId == detail[i].proId).First();
                            totalPrice += special.SpecPrice.Value * detail[i].ProductCount;
                            totalVipPrice += special.SpecPrice.Value * detail[i].ProductCount;
                        }
                        else
                        {
                            totalPrice += detail[i].UnitPrice * detail[i].ProductCount;
                            totalVipPrice += detail[i].MemberPrice * detail[i].ProductCount;
                        }
                    }
                }
                ViewBag.TotalPrice = totalPrice;
                ViewBag.MemberPriceTotal = totalVipPrice;
                //获取该店面的就餐时间
                ViewBag.Explain = "";
                ReceiveOrderModel m = new ReceiveOrderModel();
                List<ReceiveOrder2> list = m.SelReceiveOrder2Info(RestaurantId);
                if (list != null && list.Count > 0) { ViewBag.Explain = list.First().Explain; }
                return View(detail);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return Content("订单信息获取失败");
            }
        }

        /// <summary>
        /// 准备查看我的订单确认支付页面，微信登录验证
        /// </summary>
        /// <param name="MemberCardNo"></param>
        /// <param name="OrderId"></param>
        /// <param name="SourceAccountId"></param>
        /// <param name="CompanyId"></param>
        /// <param name="Type"></param>
        /// <param name="RstType"></param>
        /// <returns></returns>
        public ActionResult PrepareOrder(string MemberCardNo, string OrderId, string SourceAccountId,
            string CompanyId = null, string Type = null, string RstType = null)
        {
            const string Format = "MemberCardNo={0}&OrderId={1}&SourceAccountId={2}&CompanyId={3}&Type={4}&RstType={5}";
            string paras = string.Format(Format, MemberCardNo, OrderId, SourceAccountId, CompanyId, Type, RstType);
            var returnUrl = Constants.HostDomain + "/pay/MyOrderDetail?" + paras + "&showwxpaytitle=1";
            var state = "";
            var url = OAuthApi.GetAuthorizeUrl(TenPayV3Info.AppId, returnUrl, state, OAuthScope.snsapi_userinfo);

            return Redirect(url);
        }

        /// <summary>
        /// 1. 微信在线支付参数准备
        /// 2. 余额全额支付
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="orderId"></param>
        /// <param name="RstType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreparePay(string uid, string orderId, string RstType, string code)
        {
            string timeStamp = "";
            string nonceStr = "";
            string paySign = "";
            string package = "";
            try
            {

                PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                CrmMemberModel crmMemberModel = new CrmMemberModel();
                OrderBillPayModel billPayModel = new OrderBillPayModel();
                MyMenuModel odb = new MyMenuModel();
                OrderModel orderModel = new OrderModel();
                OrderDetailsModel orderDetaisModel = new OrderDetailsModel();

                List<MyOrderDetail> orderDetails = null;
                Models.Info.PrepayRecord prepayRecord = null;
                Models.Info.OrderBillPay billPay = null;
                Models.Info.PrepayAccount prepayAccount = null;
                Models.Info.Order order = null;

                orderDetails = odb.getMyOrderDetailListData(uid, orderId, RstType);
                order = orderModel.selOrderByOrderId(Guid.Parse(orderId)).FirstOrDefault();

                if (order != null && orderDetails != null && orderDetails.Count > 0)
                {
                    var allTodaySpecials = SpecialsModel.GetTodayByRestaurantId(order.RstId);
                    decimal totalPrice = 0;
                    decimal totalVipPrice = 0;

                    for (int i = 0; i < orderDetails.Count; i++)
                    {
                        if (allTodaySpecials.Any(x => x.ProductId == orderDetails[i].proId))
                        {
                            var special = allTodaySpecials.Where(x => x.ProductId == orderDetails[i].proId).First();
                            totalPrice += special.SpecPrice.Value * orderDetails[i].ProductCount;
                            totalVipPrice += special.SpecPrice.Value * orderDetails[i].ProductCount;
                        }
                        else
                        {
                            totalPrice += orderDetails[i].UnitPrice * orderDetails[i].ProductCount;
                            totalVipPrice += orderDetails[i].MemberPrice * orderDetails[i].ProductCount;
                        }
                    }

                    prepayAccount = crmMemberModel.GetPrepayAccount(uid);

                    bool isOnlinePay = false;
                    string openid = "";

                    //会员价
                    if (prepayAccount.AccountMoney > 0)
                    {
                        totalPrice = totalVipPrice;
                    }

                    if ((prepayAccount.AccountMoney + prepayAccount.PresentMoney) >= totalPrice)
                    {
                        // 余额支付
                        isOnlinePay = false;
                    }
                    else
                    {
                        //微信支付
                        isOnlinePay = true;

                        if (string.IsNullOrEmpty(code))
                        {
                            throw new ArgumentException("您拒绝了授权");
                        }
                        //通过，用code换取access_token
                        var openIdResult = OAuthApi.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                        if (openIdResult.errcode != ReturnCode.请求成功)
                        {
                            throw new ArgumentException("微信AccessToken错误，" + openIdResult.errcode);
                        }
                        openid = openIdResult.openid;
                    }

                    prepayRecord = new PrepayRecord();
                    prepayRecord.AddMoney = -totalPrice;//总共订单消费金额
                    prepayRecord.AsureDate = DateTime.Now;//确认时间
                    prepayRecord.BillPayId = Guid.NewGuid();//支付记录编号
                    prepayRecord.DiscountlMoeny = 0;//未启用
                    prepayRecord.PayByScore = 0;//未启用
                    prepayRecord.PayModel = "02";//微信支付
                    prepayRecord.PrepayDate = DateTime.Now;//消费记录生成时间
                    prepayRecord.PrepayMoney = -totalPrice;//总共订单消费金额，同AddMoney使用
                    prepayRecord.PresentMoney = 0;//未启用
                    prepayRecord.PromotionId = 0;//未启用
                    prepayRecord.RecMoney = 0;//未启用
                    prepayRecord.RecordId = -1;//主键，自动生成的ID
                    prepayRecord.RState = "00";//支付状态，未支付。支付成功后更新为01
                    prepayRecord.RstId = Constants.CompanyId;//未启用
                    prepayRecord.ScoreVip = 0;//未启用
                    prepayRecord.SId = orderId;//消费的订单ID
                    prepayRecord.Uid = uid;//用户ID
                    prepayRecord.UserId = "System";//未启用

                    decimal creditCard = totalPrice - (prepayAccount.AccountMoney + prepayAccount.PresentMoney);
                    if (creditCard < 0) creditCard = 0;

                    billPay = new OrderBillPay();
                    billPay.Cash = totalPrice - creditCard; //余额支付金额
                    billPay.Change = 0; // 未启用
                    billPay.Coupons = 0; // 未启用
                    billPay.CouponsNo = ""; // 未启用
                    billPay.CreateDate = DateTime.Now;//创建时间
                    billPay.CreditCard = creditCard;//在线支付金额
                    billPay.Discount = 0; // 未启用
                    billPay.MemberCard = 0; // 未启用
                    billPay.MemberCardNo = ""; // 未启用
                    billPay.PaidIn = totalPrice; // 未启用
                    billPay.PayId = prepayRecord.BillPayId.Value;//消费记录编号
                    billPay.PayState = BillPayState.NotPaid;//未支付
                    billPay.Receivable = totalPrice; // 未启用
                    billPay.Remark = "消费订单：" + orderId;//备注信息
                    billPay.Remove = 0; // 未启用
                    billPay.RstId = Constants.CompanyId;//未启用
                    billPay.UserId = Guid.Empty; // 未启用
                    billPay.UserName = orderDetails.First().ContactName;//用户姓名

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        if (!prepayRecordModel.AddPrepayRecord(prepayRecord))
                        {
                            throw new Exception("消费记录插入失败");
                        }

                        if (!billPayModel.AddBillPay(billPay))
                        {
                            throw new Exception("交易订单数据记录插入失败");
                        }

                        if (isOnlinePay)
                        {
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
                            packageReqHandler.SetParameter("body", billPay.Remark);    //商品信息
                            packageReqHandler.SetParameter("out_trade_no", billPay.PayId.ToString("N"));		//商家订单号
                            packageReqHandler.SetParameter("total_fee", (Convert.ToInt32(billPay.CreditCard * 100)).ToString());			        //商品金额,以分为单位(money * 100).ToString()
                            packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                            packageReqHandler.SetParameter("notify_url", ConfigurationManager.AppSettings["TenPayV3_TenpayNotify_Consuming"]);		    //接收财付通通知的URL
                            packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());	                    //交易类型
                            packageReqHandler.SetParameter("openid", openid);	                    //用户的openId

                            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                            packageReqHandler.SetParameter("sign", sign);	                    //签名

                            string data = packageReqHandler.ParseXML();

                            var result = TenPayV3.Unifiedorder(data);
                            var res = XDocument.Parse(result);
                            var prepayId = res.Element("xml").Element("prepay_id").Value;
                            package = string.Format("prepay_id={0}", prepayId);
                            //设置支付参数
                            RequestHandler paySignReqHandler = new RequestHandler(null);
                            paySignReqHandler.SetParameter("appId", TenPayV3Info.AppId);
                            paySignReqHandler.SetParameter("timeStamp", timeStamp);
                            paySignReqHandler.SetParameter("nonceStr", nonceStr);
                            paySignReqHandler.SetParameter("package", package);
                            paySignReqHandler.SetParameter("signType", "MD5");
                            paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);

                            //ViewData["appId"] = TenPayV3Info.AppId;
                            //ViewData["timeStamp"] = timeStamp;
                            //ViewData["nonceStr"] = nonceStr;
                            //ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                            //ViewData["paySign"] = paySign;

                            scope.Complete();
                            var onlinePayResult = new
                            {
                                IsSuccess = true,
                                Message = "",
                                appId = TenPayV3Info.AppId,
                                timeStamp = timeStamp,
                                nonceStr = nonceStr,
                                package = package,
                                paySign = paySign
                            };

                            return Json(onlinePayResult, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            Logger.Log(LoggingLevel.WxPay, "余额支付订单：" + orderId);
                            //余额支付
                            decimal accountMoney = prepayAccount.AccountMoney;
                            decimal presentMoney = prepayAccount.PresentMoney;
                            decimal cash = billPay.Cash;

                            if (presentMoney >= cash)
                            {
                                //如果赠送金额足够支付
                                //Logger.Log("presentMoney >= cash");
                                prepayAccount.PresentMoney = presentMoney - cash;
                            }
                            else if (accountMoney >= cash)
                            {
                                //如果充值余额已经足够支付
                                //Logger.Log("accountMoney >= cash");
                                prepayAccount.AccountMoney = accountMoney - cash;
                            }
                            else
                            {
                                // 优先使用赠送金额，不够的用充值余额支付
                                //Logger.Log("accountMoney + presentMoney >= cash");
                                prepayAccount.PresentMoney = 0;
                                prepayAccount.AccountMoney -= cash - presentMoney;
                            }

                            //Logger.Log("prepayAccount.AccountMoney: " + prepayAccount.AccountMoney);
                            //Logger.Log("prepayAccount.PresentMoney: " + prepayAccount.PresentMoney);

                            if (!crmMemberModel.UpdatePrepayAccount(prepayAccount))
                            {
                                throw new Exception("更新账户余额失败");
                            }

                            //Logger.Log(LoggingLevel.WxPay, "账户余额更新成功");

                            if (!orderModel.UpdateOrderStatus(Guid.Parse(orderId), OrderStatus.Paid))
                            {
                                throw new Exception("更新更新订单状态为已支付失败");
                            }

                            if (!billPayModel.UpdateBillStateAsPaid(billPay.PayId, ""))
                            {
                                throw new Exception("更新交易流水订单为已支付失败");
                            }
                            //Logger.Log(LoggingLevel.WxPay, "更新更新订单状态为已支付成功");
                            scope.Complete();

                            // Logger.Log(LoggingLevel.WxPay, "提交事务成功");

                            var offlinePayResult = new { IsSuccess = true, Message = "OfflinePay" };
                            return Json(offlinePayResult, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    throw new Exception("订单数据获取失败");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.WxPay, "调用订单支付：" + ex.ToString());
                var failedData = new { IsSuccess = false, Message = "支付失败，请重新尝试。如多次遇到此问题，请联系客服" };
                return Json(failedData, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 微信在线支付订单异步回调处理方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ComsumingPayNotifyUrl()
        {
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");
            return_code = "FAIL";
            return_msg = "处理支付通知出现异常";
            string res = null;

            try
            {
                resHandler.SetKey(TenPayV3Info.Key);
                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign())
                {
                    //正确的订单处理

                    string out_trade_no = resHandler.GetParameter("out_trade_no");
                    string total_fee = resHandler.GetParameter("total_fee");
                    //微信支付订单号
                    string transaction_id = resHandler.GetParameter("transaction_id");
                    //支付完成时间
                    string time_end = resHandler.GetParameter("time_end");

                    OrderBillPay billPay = null;
                    OrderBillPayModel billPayModel = new OrderBillPayModel();

                    Guid billPayId = Guid.Parse(out_trade_no);
                    billPay = billPayModel.GetBillPayById(billPayId);

                    if (billPay == null)
                    {
                        res += "订单:" + billPayId + "不存在";
                        return_msg = res;
                    }
                    else if (billPay.CreditCard != (decimal)(Convert.ToDecimal(total_fee) / 100))
                    {
                        res += "订单金额不符合";
                        return_msg = res;
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
                                OrderModel orderModel = new OrderModel();

                                //查询支付订单对应的充值记录
                                PrepayRecord prepayRecord = prepayRecordModel.GetPrepayRecordByBillPayId(billPayId);

                                if (prepayRecord == null)
                                {
                                    res += "查询不到对应的消费记录";
                                    return_msg = res;
                                }
                                else
                                {
                                    string uid = prepayRecord.Uid;
                                    //查询个人余额账户
                                    PrepayAccount prepayAccount = crmMemberModel.GetPrepayAccount(prepayRecord.Uid);

                                    if (prepayAccount == null)
                                    {
                                        res += "查询不到用户账户记录";
                                        return_msg = res;
                                    }
                                    else
                                    {
                                        //个人账户清零                               
                                        prepayAccount.LastPresentMoney = prepayRecord.PresentMoney;
                                        prepayAccount.AccountMoney = 0;
                                        prepayAccount.PresentMoney = 0;
                                        prepayAccount.TotalPresent = 0;
                                        prepayAccount.TotalMoney = 0;
                                        prepayAccount.LastConsumeDate = DateTime.Now;
                                        prepayAccount.LastConsumeMoney = billPay.Cash;

                                        //更新支付时间和状态
                                        prepayRecord.RState = "01";
                                        prepayRecord.AsureDate = DateTime.Now;

                                        if (!prepayRecordModel.UpdatePrepayRecord(prepayRecord))
                                        {
                                            res += "更新消费记录失败";
                                        }
                                        else if (!crmMemberModel.UpdatePrepayAccount(prepayAccount))
                                        {
                                            res += "更新用户账户信息未成功";
                                        }
                                        else if (!orderModel.UpdateOrderStatus(Guid.Parse(prepayRecord.SId), OrderStatus.Paid))
                                        {
                                            res += "更新订单状态为已支付失败";
                                        }
                                        else
                                        {
                                            return_msg = res;
                                            return_code = "SUCCESS";
                                            return_msg = "OK";
                                            //提交事务
                                            scope.Complete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        res = "订单：" + billPay.PayId + "已处理，不能重复处理";
                        return_code = "SUCCESS";
                        return_msg = "OK";
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

            string xml = string.Format(@"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);

            Logger.Log(LoggingLevel.WxPay, res);
            Logger.Log(LoggingLevel.WxPay, xml);
            return Content(xml, "text/xml");

        }

        /// <summary>
        /// 会员支付功能页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult PayView(string id, string value)
        {
            Logger.Log(LoggingLevel.WxPay, "PayView Enter. Id=" + id + "; value=" + value);

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(value))
            {
                ViewBag.Content = "支付数据无效！";
                return View("Error");
            }
            string[] arrrec = value.Split('|');
            if (arrrec== null || arrrec.Length < 2)
            {
                ViewBag.Content = "非法链接！";
                return View("Error");
            }
            //DateTime dt = new DateTime(long.Parse(arrrec[1]));
            //if ((DateTime.Now - dt).TotalSeconds > 300) {
            //    ViewBag.Content = "链接超时无效！";
            //    return View("Error");
            //} 
            try
            {
                int recid = int.Parse(arrrec[0]);
                PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                OrderBillPayModel billPayModel = new OrderBillPayModel();
                CrmMemberModel crmMemberModel = new CrmMemberModel();

                PrepayRecord rec = prepayRecordModel.GetPrepayRecordByRecordIdAndSourceAccountId(recid, id);
                OrderBillPay billPay = billPayModel.GetBillPayById(rec.BillPayId.Value);
                PrepayAccount acc = crmMemberModel.GetPrepayAccount(rec.Uid);

                if (rec == null || rec.AddMoney > 0)
                {
                    ViewBag.Content = "记录无效！";
                    return View("Error");
                }
                rec.RState = "01";
                rec.AsureDate = DateTime.Now;
                if (billPay == null)
                {
                    decimal cash = Math.Abs(rec.PrepayMoney.Value + rec.PresentMoney.Value);

                    billPay = new OrderBillPay();
                    //余额支付金额
                    billPay.Cash = cash;
                    billPay.Change = 0;
                    billPay.Coupons = 0;
                    billPay.CouponsNo = "";
                    billPay.CreateDate = DateTime.Now;
                    //在线支付金额
                    billPay.CreditCard = 0;
                    billPay.Discount = 0;
                    billPay.MemberCard = 0;
                    billPay.MemberCardNo = "";
                    billPay.PaidIn = cash;
                    billPay.PayId = rec.BillPayId.HasValue ? rec.BillPayId.Value : Guid.NewGuid();
                    billPay.PayState = BillPayState.NotPaid;
                    billPay.Receivable = cash;
                    billPay.Remark = "消费订单：" + rec.SId;
                    billPay.Remove = 0;
                    billPay.RstId = Constants.CompanyId;
                    billPay.UserId = Guid.Empty;
                    billPay.UserName = "";

                    rec.BillPayId = rec.BillPayId ?? billPay.PayId;

                    billPayModel.AddBillPay(billPay);
                    prepayRecordModel.UpdatePrepayRecord(rec);
                }

                if (billPay.PayState == BillPayState.NotPaid)
                {
                    if ((DateTime.Now - rec.PrepayDate.Value).TotalSeconds > 300)
                    {
                        ViewBag.Content = "支付记录超时，请联系收银员重新发起结单请求！";
                        return View("Error");
                    }

                    // acc.AccountMoney = acc.AccountMoney + rec.AddMoney;
                    //acc.LastConsumeMoney = 0 - rec.AddMoney;
                    //acc.LastConsumeDate = DateTime.Now;

                    if (rec.PrepayMoney + rec.PresentMoney + acc.AccountMoney + acc.PresentMoney < 0)
                    {
                        ViewBag.Content = "余额不足！";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.Content = "该订单已支付！";
                    return View("Error");
                }
                ViewBag.Id = id;
                ViewBag.RecId = rec.RecordId;
                ViewBag.DateTimeTicks = DateTime.Now.Ticks;
                ViewBag.PayState = billPay != null ? billPay.PayState : "0x02";
                ViewBag.OrderId = rec.SId;

                return View(rec);
            }
            catch(Exception ex)
            {
                Logger.Log(LoggingLevel.WxPay, ex);
                ViewBag.Content = "系统错误！";
                return View("Error");
                //return Content("<strong font-size='14'>系统错误！</strong>");
            }
        }

        /// <summary>
        /// 会员支付页面处理方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recid"></param>
        /// <param name="dateTimeTicks"></param>
        /// <returns></returns>
        public ActionResult PayAsure(string id, int recid, long dateTimeTicks)
        {
            Logger.Log(LoggingLevel.WxPay, "PayAsure Enter. Id=" + id + "; value=" + recid + "|" +dateTimeTicks.ToString());

            if (string.IsNullOrEmpty(id) || recid == 0 || dateTimeTicks == 0)
            {
                return Content("非法链接！");
            }

            DateTime dt = new DateTime(dateTimeTicks);
            if ((DateTime.Now - dt).TotalSeconds > 300)
            {
                return Content("链接超时无效！");
            }

            try
            {
                PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                OrderBillPayModel billPayModel = new OrderBillPayModel();
                CrmMemberModel crmMemberModel = new CrmMemberModel();
                OrderModel orderModel = new OrderModel();

                PrepayRecord rec = prepayRecordModel.GetPrepayRecordByRecordIdAndSourceAccountId(recid, id);
                OrderBillPay billPay = billPayModel.GetBillPayById(rec.BillPayId.Value);
                PrepayAccount acc = crmMemberModel.GetPrepayAccount(rec.Uid);
                Models.Info.Order order = orderModel.selOrderByOrderId(Guid.Parse(rec.SId)).FirstOrDefault();

                if (rec == null || rec.AddMoney > 0 || order == null)
                {
                    return Content("记录无效！");
                }

                if ((billPay != null && billPay.PayState == BillPayState.Paid) || order.Status == OrderStatus.Paid)
                {
                    return Content("该账单已支付！");
                }

                if ((DateTime.Now - rec.PrepayDate.Value).TotalSeconds > 300)
                {
                    return Content("支付记录超时，请联系收银员重新发起结单请求！");
                }

                rec.RState = "01";
                rec.AsureDate = DateTime.Now;

                if (acc.AccountMoney + rec.PrepayMoney < 0 || acc.PresentMoney + rec.PresentMoney < 0)
                {
                    return Content("余额不足");
                }

                //acc.AccountMoney = acc.AccountMoney + rec.PrepayMoney.Value;
                //acc.LastConsumeMoney = 0 - rec.AddMoney;
                //acc.PresentMoney = acc.PresentMoney + rec.PresentMoney.Value;
                //acc.LastConsumeDate = DateTime.Now;

                decimal cash = Math.Abs(rec.PrepayMoney.Value + rec.PresentMoney.Value);

                bool result = true;
                if (billPay == null)
                {
                    decimal totalPrice = cash;

                    billPay = new OrderBillPay();
                    //余额支付金额
                    billPay.Cash = cash;
                    billPay.Change = 0;
                    billPay.Coupons = 0;
                    billPay.CouponsNo = "";
                    billPay.CreateDate = DateTime.Now;
                    //在线支付金额
                    billPay.CreditCard = 0;
                    billPay.Discount = 0;
                    billPay.MemberCard = 0;
                    billPay.MemberCardNo = "";
                    billPay.PaidIn = totalPrice;
                    billPay.PayId = rec.BillPayId.HasValue ? rec.BillPayId.Value : Guid.NewGuid();

                    rec.BillPayId = rec.BillPayId ?? billPay.PayId;

                    billPay.PayState = BillPayState.Paid;
                    billPay.Receivable = totalPrice;
                    billPay.Remark = "消费订单：" + rec.SId;
                    billPay.Remove = 0;
                    billPay.RstId = Constants.CompanyId;
                    billPay.UserId = Guid.Empty;
                    billPay.UserName = "";

                    result = billPayModel.AddBillPay(billPay);
                }

                Logger.Log(LoggingLevel.WxPay, "余额支付订单：" + order.Id);
                //余额支付
                decimal accountMoney = acc.AccountMoney;
                decimal presentMoney = acc.PresentMoney;

                if (presentMoney >= cash)
                {
                    //如果赠送金额足够支付
                    //Logger.Log("presentMoney >= cash");
                    acc.PresentMoney = presentMoney - cash;
                }
                else if (accountMoney >= cash)
                {
                    //如果充值余额已经足够支付
                    //Logger.Log("accountMoney >= cash");
                    acc.AccountMoney = accountMoney - cash;
                }
                else
                {
                    // 优先使用赠送金额，不够的用充值余额支付
                    //Logger.Log("accountMoney + presentMoney >= cash");
                    acc.PresentMoney = 0;
                    acc.AccountMoney -= cash - presentMoney;
                }

                result = result && prepayRecordModel.UpdatePrepayRecord(rec);
                result = result && crmMemberModel.UpdatePrepayAccount(acc);
                result = result && billPayModel.UpdateBillStateAsPaid(billPay.PayId, "");
                result = result && orderModel.UpdateOrderStatus(order.Id, OrderStatus.Paid);

                if (result)
                {
                    WxApiClass wxapi = new WxApiClass();
                    if (wxapi.Logon())
                    {
                        wxapi.sendMessage("尊敬的会员：" + acc.uid + ",您已经支付订单：" + rec.SId
                             + ",本次会员消费为您节省：￥" + rec.DiscountlMoeny.Value.ToString("F2")
                            + ",支付：现金￥" + (0 - rec.PrepayMoney.Value).ToString("F2")
                            + "/赠送￥" + (0 - rec.PresentMoney.Value).ToString("F2")
                            + "/积分" + rec.PayByScore.Value.ToString("F2")
                            + "。您的剩余金额为：现金" + acc.AccountMoney.ToString("F2")
                            + "/赠送" + acc.PresentMoney.ToString("F2")
                            //+ "，当前积分：" + currScore.ToString("F2")
                            //+ (currScore > 50 ? "，您的积分大于50，下次可以使用积分消费！" : "")
                            , id);
                    }

                    ViewBag.SourceAccountId = id;
                    ViewBag.CompanyId = Constants.CompanyId;
                    ViewBag.OrderId = order.Id;

                    return Content("success");
                }
                else
                {
                    return Content("确认支付失败！错误号：1000");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.WxPay, "PayAsure, ID=" + id + ", value=" + recid + "\r\n" + dateTimeTicks + "\r\n" + ex);
                return Content("确认支付失败！错误号：9900");
            }
        }

        #endregion Order
    }
}
