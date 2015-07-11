using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Member/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult member(string id, string name)
        {
            List<CrmMember> p = null;
            CrmMemberModel cdb = new CrmMemberModel();
            p = cdb.getCrmMemberListInfoData(name);
            ViewBag.SourceAccountId = name;
            ViewBag.CompanyId = id;
            return View(p);
        }

        public ActionResult Info(string id, string name)
        {
            CrmMember model = null;

            ViewBag.AccountMoney = 0;
            ViewBag.Score = 0;
            ViewBag.PresentMoney = 0;
            ViewBag.CompanyId = id;
            ViewBag.SourceAccountId = name;

            try
            {
                CrmMemberModel cmModel = new CrmMemberModel();
                CrmMemberScoreModel cmsModel = new CrmMemberScoreModel();

                if (!string.IsNullOrEmpty(name))
                {
                    model = cmModel.getCrmMemberListInfoData(name).FirstOrDefault();

                    var prepayAccount = cmModel.GetPrepayAccount(model.Uid);
                    var memberScore = cmsModel.SelCrmMemberScoreInfo(model.Uid).FirstOrDefault();

                    if (prepayAccount != null && memberScore != null)
                    {
                        ViewBag.AccountMoney = prepayAccount.AccountMoney;
                        ViewBag.PresentMoney = prepayAccount.PresentMoney;
                        ViewBag.Score = memberScore.Score;
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }

            return View(model);
        }

        public ActionResult editMemberInfo(string id, string name)
        {
            List<CrmMember> p = null;
            try
            {
                CrmMemberModel cmm = new CrmMemberModel();
                p = cmm.getCrmMemberListInfoData(name);
                decimal dec = cmm.GetPrepayAccount(p.First().Uid).AccountMoney;
                ViewBag.PrepayAccount = dec;
                ViewBag.SourceAccountId = name;
                ViewBag.CompanyId = id;
            }
            catch
            { }
            finally
            { }

            return View(p);
        }

        public ActionResult SaveMem(string CompanyId, string SourceAccountId, string id, string name, string phone, string sex, string bir, string addr)
        {
            CrmMemberModel cdb = new CrmMemberModel();
            int i = cdb.Save(id, name, phone, sex, bir, addr);
            if (i == 1)
            {

            }
            return RedirectToAction("Info", "Member", new { id = CompanyId, name = SourceAccountId });
        }

        /// <summary>
        /// 消费记录
        /// </summary>
        /// <param name="id">Uid</param>
        /// <param name="name">SourceAccountId</param>
        /// <param name="companyId">CompanyId</param>
        /// <returns></returns>
        public ActionResult ConsumptionRecords(string id, string name, string companyId)
        {
            List<PrepayRecord> p = null;
            try
            {
                CrmMemberModel cmm = new CrmMemberModel();
                List<CrmMember> crm = cmm.getCrmMemberListInfoData(name);
                ViewBag.PrepayAccount = 0;
                if (crm.Count() > 0)
                {
                    decimal dec = cmm.GetPrepayAccount(crm.First().Uid).AccountMoney;
                    ViewBag.PrepayAccount = dec;
                }
                ViewBag.Uid = id;
                ViewBag.CompanyId = companyId;
                PrepayRecordModel prm = new PrepayRecordModel();
                p = prm.getConsumptionRecordsListInfoData(id);
            }
            catch
            {

            }
            finally
            {

            }

            return View(p);
        }

        //充值记录
        public ActionResult RechargeRecord(string id, string name, string companyId)
        {
            List<PrepayRecord> p = null;
            try
            {
                ViewBag.Uid = id;
                PrepayRecordModel prm = new PrepayRecordModel();
                p = prm.getRechargeRecordListInfoData(id);
            }
            catch
            {

            }
            finally
            {

            }
            return View(p);
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
                //这里需要重新设置一下充值支付返回处理路径
                _tenPayV3Info.TenPayV3Notify = ConfigurationManager.AppSettings["TenPayV3_TenpayNotify"];
                return _tenPayV3Info;
            }
        }

        public ActionResult PreRecharge(string id, string name)
        {
            var returnUrl = "/member/recharge?name=" + name + "&showwxpaytitle=1";
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
            catch
            {

            }
            finally
            {
            }

            return View(crmMember);
        }

        [HttpPost]
        public ActionResult Recharge(string code, string uid, string name, string tel, string amount, bool isFirstRecharge)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    var jsonData = new { IsSuccess = false, Message = "您拒绝了授权" };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }

                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";


                //通过，用code换取access_token
                var openIdResult = OAuthApi.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    var jsonData = new { IsSuccess = false, Message = "错误：" + openIdResult.errmsg };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }

                PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                CrmMemberModel crmMemberModel = new CrmMemberModel();
                BillPayModel billPayModel = new BillPayModel();

                Models.Info.PrepayRecord prepayRecord = null;
                Models.Info.BillPay billPay = null;

                decimal totalPrice = 0;
                decimal presentMoney = 0;

                if (!decimal.TryParse(amount, out totalPrice))
                {
                    var jsonData = new { IsSuccess = false, Message = "金额错误" };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }

                if (isFirstRecharge && totalPrice != 1000)
                {
                    var jsonData = new { IsSuccess = false, Message = "金额错误" };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }

                if (!isFirstRecharge && totalPrice != 500 && totalPrice != 1000)
                {
                    var jsonData = new { IsSuccess = false, Message = "金额错误" };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }

                //try
                //{
                //    //强制设置为配置的值。
                //    string testingAmount = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_Testing_Amount"];
                //    decimal.TryParse(testingAmount, out totalPrice);
                //}
                //catch
                //{

                //}

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
                prepayRecord.RState = "";
                prepayRecord.RstId = Constants.CompanyId;
                prepayRecord.ScoreVip = 0;
                prepayRecord.SId = Guid.NewGuid().ToString();//DateTime.Now.ToString("HHmmss") + TenPayV3Util.BuildRandomStr(28);
                prepayRecord.Uid = uid;
                prepayRecord.UserId = "System";

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
                billPay.Remark = "用户充值" + totalPrice + "元";
                billPay.Remove = 0;
                billPay.RstId = Constants.CompanyId;
                billPay.UserId = Guid.Empty;
                billPay.UserName = name;

                if (!prepayRecordModel.AddPrepayRecord(prepayRecord))
                {
                    var jsonData = new { IsSuccess = false, Message = "充值数据插入失败" };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }

                if (!billPayModel.AddBillPay(billPay))
                {
                    var jsonData = new { IsSuccess = false, Message = "充值数据插入失败" };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                var jsonData = new { IsSuccess = false, Message = "请求发生错误，请返回重新尝试.\r\n" + ex.Message };//"请求发生错误，请返回重新尝试" };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
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

                                        //更新支付时间
                                        //prepayRecord.AsureDate = DateTime.Now;

                                        if (crmMemberModel.UpdatePrepayAccount(prepayAccount))
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

            var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/1.txt"));
            fileStream.Write(Encoding.Default.GetBytes(res), 0, Encoding.Default.GetByteCount(res));
            fileStream.Close();

            string xml = string.Format(@"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);

            return Content(xml, "text/xml");
        }

    }
}
