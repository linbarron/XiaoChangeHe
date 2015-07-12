using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe.Models.SubmitOrder;

namespace WitBird.XiaoChangHe.Controllers
{
    public class MyMenuController : Controller
    {
        //
        // GET: /MyMenu/

        public ActionResult MyMenu(string MemberCardNo, string OrderId, string SourceAccountId, string RstType = null, string ComypanyId = null)
        {

            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;
            string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.RestaurantId = RestaurantId;
            if (OrderId != null && OrderId != "")
            {
                MyMenuModel odb = new MyMenuModel();

                List<MyMenu> mymenu = odb.getMyMenuListData(MemberCardNo, OrderId, RstType);

                ViewBag.MyMenuListData = mymenu;
                decimal sum = 0;
                if (mymenu.Count > 0)
                {
                    for (int i = 0; i < mymenu.Count; i++)
                    {
                        sum += mymenu[i].UnitPrice * mymenu[i].ProductCount;
                    }
                }
                ViewBag.total = sum;

            }
            ViewBag.RstType = RstType;
            ViewBag.ComypanyId = ComypanyId;

            return View();
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
                //这里需要重新设置一下消费支付返回处理路径
                _tenPayV3Info.TenPayV3Notify = ConfigurationManager.AppSettings["TenPayV3_TenpayNotify_Consuming"];
                return _tenPayV3Info;
            }
        }

        public ActionResult MyOrderDetail(string code, string state, string MemberCardNo, string OrderId, string SourceAccountId,
            string ComypanyId = null, string Type = null, string RstType = null)
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

                decimal dec = cdb1.GetPrepayAccount(MemberCardNo).AccountMoney;
                ViewBag.PrepayAccount = dec;

                string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
                ViewBag.RestaurantId = RestaurantId;

                List<MyOrderDetail> detail = odb.getMyOrderDetailListData(MemberCardNo, OrderId, RstType);
                // ViewBag.MyOrderDetail = detail;

                decimal sum = 0;
                if (detail.Count > 0)
                {
                    for (int i = 0; i < detail.Count; i++)
                    {
                        if (detail[i].UnitPrice != 0)
                        {
                            sum += detail[i].MemberPrice * detail[i].ProductCount;
                        }
                    }
                }
                ViewBag.MemberPriceTotal = sum;
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

        public ActionResult PrepareOrder(string MemberCardNo, string OrderId, string SourceAccountId,
            string ComypanyId = null, string Type = null, string RstType = null)
        {
            const string Format = "MemberCardNo={0}&OrderId={1}&SourceAccountId={2}&ComypanyId={3}&Type={4}&RstType={5}";
            string paras = string.Format(Format, MemberCardNo, OrderId, SourceAccountId, ComypanyId, Type, RstType);
            var returnUrl = Constants.HostDomain + "/mymenu/MyOrderDetail?" + paras + "&showwxpaytitle=1";
            var state = "";
            var url = OAuthApi.GetAuthorizeUrl(TenPayV3Info.AppId, returnUrl, state, OAuthScope.snsapi_userinfo);

            return Redirect(url);
        }

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
                BillPayModel billPayModel = new BillPayModel();
                MyMenuModel odb = new MyMenuModel();
                OrderModel orderModel = new OrderModel();
                OrderDetailsModel orderDetaisModel = new OrderDetailsModel();

                List<MyOrderDetail> orderDetails = null;
                Models.Info.PrepayRecord prepayRecord = null;
                Models.Info.BillPay billPay = null;
                Models.Info.PrepayAccount prepayAccount = null;

                orderDetails = odb.getMyOrderDetailListData(uid, orderId, RstType);

                if (orderDetails != null && orderDetails.Count > 0)
                {
                    decimal totalPrice = 0;
                    decimal totalVipPrice = 0;

                    for (int i = 0; i < orderDetails.Count; i++)
                    {
                        if (orderDetails[i].UnitPrice != 0)
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
                    prepayRecord.AddMoney = -totalPrice;
                    prepayRecord.AsureDate = DateTime.Now;
                    prepayRecord.BillPayId = Guid.NewGuid();
                    prepayRecord.DiscountlMoeny = 0;
                    prepayRecord.PayByScore = 0;
                    prepayRecord.PayModel = "02";
                    prepayRecord.PrepayDate = DateTime.Now;
                    prepayRecord.PrepayMoney = -totalPrice;
                    prepayRecord.PresentMoney = 0;
                    prepayRecord.PromotionId = 0;
                    prepayRecord.RecMoney = 0;
                    prepayRecord.RecordId = -1;
                    prepayRecord.RState = "";
                    prepayRecord.RstId = Constants.CompanyId;
                    prepayRecord.ScoreVip = 0;
                    prepayRecord.SId = orderId;
                    prepayRecord.Uid = uid;
                    prepayRecord.UserId = "System";

                    decimal creditCard = totalPrice - (prepayAccount.AccountMoney + prepayAccount.PresentMoney);
                    if (creditCard < 0) creditCard = 0;

                    billPay = new BillPay();
                    //余额支付金额
                    billPay.Cash = totalPrice - creditCard;
                    billPay.Change = 0;
                    billPay.Coupons = 0;
                    billPay.CouponsNo = "";
                    billPay.CreateDate = DateTime.Now;
                    //在线支付金额
                    billPay.CreditCard = creditCard;
                    billPay.Discount = 0;
                    billPay.MemberCard = 0;
                    billPay.MemberCardNo = "";
                    billPay.PaidIn = totalPrice;
                    billPay.PayId = prepayRecord.BillPayId.Value;
                    billPay.PayState = BillPayState.NotPaid;
                    billPay.Receivable = totalPrice;
                    billPay.Remark = "消费订单：" + orderId;
                    billPay.Remove = 0;
                    billPay.RstId = Constants.CompanyId;
                    billPay.UserId = Guid.Empty;
                    billPay.UserName = orderDetails.First().ContactName;

                    billPay.CreditCard = 0.01m;

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
                            //余额支付
                            decimal accountMoney = prepayAccount.AccountMoney;
                            decimal presentMoney = prepayAccount.PresentMoney;
                            decimal cash = billPay.Cash;

                            Logger.Log("accountMoney: " + accountMoney);
                            Logger.Log("presentMoney: " + presentMoney);
                            Logger.Log("cash: " + cash);
                            
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

                            if (!crmMemberModel.UpdatePrepayAccount(prepayAccount) && orderModel.UpdateOrderAsPaid(Guid.Parse(orderId)))
                            {
                                throw new Exception("更新账户余额失败或者更新订单状态为已支付失败");
                            }
                            else
                            {
                                scope.Complete();
                                var offlinePayResult = new { IsSuccess = true, Message = "OfflinePay" };
                                return Json(offlinePayResult, JsonRequestBehavior.AllowGet);
                            }
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
                Logger.Log("调用订单支付：" + ex.ToString());
                var failedData = new { IsSuccess = false, Message = "支付请求失败，请重新尝试。如多次遇到此问题，请联系客服" };
                return Json(failedData, JsonRequestBehavior.AllowGet);
            }
        }

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

                    BillPay billPay = null;
                    BillPayModel billPayModel = new BillPayModel();

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
                                OrderModel orderModel = new OrderModel();

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
                                        //个人账户清零                               
                                        prepayAccount.LastPresentMoney = prepayRecord.PresentMoney;
                                        prepayAccount.AccountMoney = 0;
                                        prepayAccount.PresentMoney = 0;
                                        prepayAccount.TotalPresent = 0;
                                        prepayAccount.TotalMoney = 0;
                                        prepayAccount.LastConsumeDate = DateTime.Now;
                                        prepayAccount.LastConsumeMoney = billPay.Cash;

                                        if (!crmMemberModel.UpdatePrepayAccount(prepayAccount))
                                        {
                                            res = "更新用户账户信息未成功";
                                        }
                                        else if(!orderModel.UpdateOrderAsPaid(Guid.Parse(prepayRecord.SId)))
                                        {
                                            res = "更新订单状态为已支付失败";
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

        public int EmptyMyMenu(string MemberCardNo, string orderId, string SourceAccountId, string type = null)
        {
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = orderId;
            ViewBag.MemberCardNo = MemberCardNo;
            OrderDetailsModel odm = new OrderDetailsModel();
            OrderModel om = new OrderModel();
            //  List<OrderDetails> list = odm.getOrderDetaiById(orderId);
            int i;
            //if (list==null&&list.Count== 0)
            //{
            //    i = om.EmptyOrder(orderId);
            //}
            //else
            //{emptyOrdersDetail

            i = odm.EmptyOrderDetails(MemberCardNo, orderId);
            //  if (i == 1)
            //  {
            if (string.IsNullOrEmpty(type))
            {
                i = om.EmptyOrder(orderId);
            }
            // }
            // }
            return i;
            // return RedirectToAction("MyMenu", "MyMenu", new { MemberCardNo = MemberCardNo, OrderId = orderId, SourceAccountId = SourceAccountId });
        }

        public ActionResult MyAutoMenus(string MemberCardNo, string OrderId, string SourceAccountId, string RstType, string Type = null, string peopleCount = null)
        {

            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;
            ViewBag.RstType = RstType;
            ViewBag.Type = Type;
            ViewBag.peopleCount = peopleCount;
            string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.RestaurantId = RestaurantId;
            List<MyMenu> mymenu = null;
            MyMenuModel odb = new MyMenuModel();
            if (OrderId != null && OrderId != "")
            {
                mymenu = odb.getMyMenuListData(MemberCardNo, OrderId, RstType);
                ViewBag.MyMenuListData = mymenu;
                decimal sum = 0, msum = 0;
                if (mymenu.Count > 0)
                {
                    for (int i = 0; i < mymenu.Count; i++)
                    {
                        sum += mymenu[i].UnitPrice * mymenu[i].ProductCount;
                        msum += mymenu[i].MemberPrice * mymenu[i].ProductCount;

                    }
                }
                ViewBag.total = sum;
                ViewBag.mTotal = msum;
            }
            List<PresentProduct> presentProducts = null;
            presentProducts = odb.getPresentProductListData(RestaurantId);
            ViewBag.presentProducts = null;
            if (presentProducts != null && presentProducts.Count > 0)
            {
                ViewBag.presentProducts = presentProducts;
            }

            return View(mymenu);
        }

        [HttpPost]
        public JsonResult SubmitOrder(SubmitOrderEntity order)
        {
            string msg = string.Empty;
            try
            {
                CrmMemberModel cdb1 = new CrmMemberModel();

                if (string.IsNullOrEmpty(order.SourceAccountId))
                {
                    order.SourceAccountId = Session["SourceAccountId"] as string;
                }
                var currentUser = cdb1.getCrmMemberListInfoData(order.SourceAccountId).FirstOrDefault();
                decimal dec = cdb1.GetPrepayAccount(currentUser.Uid).AccountMoney;
                if (!SubmitOrderDBModel.UpdateOrderInfo(order))
                {
                    msg = "提交订单失败！";
                }
            }
            catch (Exception ex)
            {
                msg = "提交订单失败";
                Logger.Log(LoggingLevel.Normal, ex);
            }
            return Json(msg);
        }
    }
}
