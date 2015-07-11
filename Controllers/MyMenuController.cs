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

        public ActionResult PrepareOrder(string MemberCardNo, string OrderId, string SourceAccountId,
            string ComypanyId = null, string Type = null, string RstType = null)
        {
            const string Format = "MemberCardNo={0}&OrderId={1}&SourceAccountId={2}&ComypanyId={3}&Type={4}&RstType={5}";
            string paras = string.Format(Format, MemberCardNo, OrderId, SourceAccountId, ComypanyId, Type, RstType);
            var returnUrl = "/member/MyOrderDetail?" + paras + "&showwxpaytitle=1";
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

                Models.Info.Order order = null;
                List<MyOrderDetail> orderDetails = null;
                Models.Info.PrepayRecord prepayRecord = null;
                Models.Info.BillPay billPay = null;
                Models.Info.PrepayAccount prepayAccount = null;

                order = orderModel.selOrderId(orderId).FirstOrDefault();

                if (order != null)
                {
                    decimal totalPrice = 0;

                    orderDetails = odb.getMyOrderDetailListData(uid, orderId, RstType);
                    if (orderDetails.Count > 0)
                    {
                        for (int i = 0; i < orderDetails.Count; i++)
                        {
                            if (orderDetails[i].UnitPrice != 0)
                            {
                                totalPrice += orderDetails[i].MemberPrice * orderDetails[i].ProductCount;
                            }
                        }
                    }
                    prepayAccount = crmMemberModel.GetPrepayAccount(uid);

                    bool isOnlinePay = false;
                    string openid = "";
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
                            var jsonData = new { IsSuccess = false, Message = "您拒绝了授权" };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
                        }
                        //通过，用code换取access_token
                        var openIdResult = OAuthApi.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                        if (openIdResult.errcode != ReturnCode.请求成功)
                        {
                            var jsonData = new { IsSuccess = false, Message = "支付错误：" + openIdResult.errmsg };
                            return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                    billPay.Cash = prepayAccount.AccountMoney + prepayAccount.PresentMoney;//(totalPrice - creditCard);
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
                    billPay.UserName = order.ContactName;

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        if (!prepayRecordModel.AddPrepayRecord(prepayRecord))
                        {
                            var failedData = new { IsSuccess = false, Message = "支付失败" };
                            return Json(failedData, JsonRequestBehavior.AllowGet);
                        }

                        if (!billPayModel.AddBillPay(billPay))
                        {
                            var failedData = new { IsSuccess = false, Message = "支付失败" };
                            return Json(failedData, JsonRequestBehavior.AllowGet);
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
                            packageReqHandler.SetParameter("notify_url", TenPayV3Info.TenPayV3Notify);		    //接收财付通通知的URL
                            packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());	                    //交易类型
                            packageReqHandler.SetParameter("openid", openid);	                    //用户的openId

                            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
                            packageReqHandler.SetParameter("sign", sign);	                    //签名

                            string data = packageReqHandler.ParseXML();

                            var result = TenPayV3.Unifiedorder(data);
                            var res = XDocument.Parse(result);
                            var prepayId = res.Element("xml").Element("prepay_id").Value;

                            //设置支付参数
                            RequestHandler paySignReqHandler = new RequestHandler(null);
                            paySignReqHandler.SetParameter("appId", TenPayV3Info.AppId);
                            paySignReqHandler.SetParameter("timeStamp", timeStamp);
                            paySignReqHandler.SetParameter("nonceStr", nonceStr);
                            paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                            paySignReqHandler.SetParameter("signType", "MD5");
                            paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);

                            //ViewData["appId"] = TenPayV3Info.AppId;
                            //ViewData["timeStamp"] = timeStamp;
                            //ViewData["nonceStr"] = nonceStr;
                            //ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                            //ViewData["paySign"] = paySign;


                            //余额清零，在notify里边做
                            //if (billPay.Cash > 0)
                            //{
                            //    prepayAccount.AccountMoney = 0;
                            //    prepayAccount.PresentMoney = 0;
                            //    prepayAccount.TotalMoney = 0;
                            //    prepayAccount.TotalPresent = 0;
                            //    prepayAccount.LastConsumeDate = DateTime.Now;
                            //    prepayAccount.LastConsumeMoney = billPay.Cash;
                            //    prepayAccount.LastPresentMoney = 0;

                            //    if (!crmMemberModel.UpdatePrepayAccount(prepayAccount))
                            //    {
                            //        var failedData = new { IsSuccess = false, Message = "支付失败" };
                            //        return Json(failedData, JsonRequestBehavior.AllowGet);
                            //    }
                            //}

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

                            //如果充值余额已经足够支付
                            if (accountMoney >= cash)
                            {
                                prepayAccount.AccountMoney = accountMoney - cash;
                            }
                            //如果赠送金额足够支付
                            else if (presentMoney >= cash)
                            {
                                prepayAccount.PresentMoney = presentMoney - cash;
                            }
                            else
                            {
                                // 优先使用充值余额，不够的用赠送金额支付
                                prepayAccount.AccountMoney = 0;
                                prepayAccount.PresentMoney -= cash - accountMoney;
                            }

                            if (!crmMemberModel.UpdatePrepayAccount(prepayAccount))
                            {
                                var failedData = new { IsSuccess = false, Message = "支付失败" };
                                return Json(failedData, JsonRequestBehavior.AllowGet);
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
                var jsonData = new { IsSuccess = false, Message = "请求发生错误，请返回重新尝试.\r\n" + ex.Message };//"请求发生错误，请返回重新尝试" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                CrmMemberModel cdb1 = new CrmMemberModel();
                ViewBag.PrepayAccount = 0;
                decimal dec = cdb1.GetPrepayAccount(MemberCardNo).AccountMoney;
                ViewBag.PrepayAccount = dec;
                string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
                ViewBag.RestaurantId = RestaurantId;
                MyMenuModel odb = new MyMenuModel();

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
                return Content("订单信息获取错误:\r\rn" + ex.Message);
            }
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
                var currentUser = Session["CrmMember"] as CrmMember;
                CrmMemberModel cdb1 = new CrmMemberModel();
                decimal dec = cdb1.GetPrepayAccount(currentUser.Uid).AccountMoney;
                if (!SubmitOrderDBModel.UpdateOrderInfo(order))
                {
                    msg = "提交订单失败！";
                }
            }
            catch (Exception ex)
            {
                msg = "出错了！";
            }
            return Json(msg);
        }
    }
}
