using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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

                    var prepayAccount = cmModel.getPrepayAccount(model.Uid).FirstOrDefault();
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
                decimal dec = cmm.getPrepayAccount(p.First().Uid).First().AccountMoney;
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
                    decimal dec = cmm.getPrepayAccount(crm.First().Uid).First().AccountMoney;
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
                CrmMemberModel cmm = new CrmMemberModel();
                List<CrmMember> crm = cmm.getCrmMemberListInfoData(name);
                ViewBag.PrepayAccount = 0;
                if (crm.Count() > 0)
                {
                    decimal dec = cmm.getPrepayAccount(crm.First().Uid).First().AccountMoney;
                    ViewBag.PrepayAccount = dec;
                }
                ViewBag.Uid = id;
                ViewBag.CompanyId = companyId;
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

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

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

        public ActionResult PreRecharge(string id, string name)
        {
            Session["SourceAccountId"] = name;
            var returnUrl = string.Format("http://test.xgdg.cn/member/recharge");
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
        public ActionResult Recharge(string code, string state)
        {
            CrmMember crmMember = null;

            try
            {
                string name = Session["SourceAccountId"].ToString();
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
                    return Content("您拒绝了授权！");
                }

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
                decimal presentMoney = 0;

                if (!decimal.TryParse(amount, out totalPrice))
                {
                    return Content("金额错误");
                }

                if (isFirstRecharge && totalPrice != 1000)
                {
                    return Content("金额错误");
                }

                if (!isFirstRecharge && (totalPrice != 500 || totalPrice != 1000))
                {
                    return Content("金额错误");
                }

                prepayAccount = crmMemberModel.getPrepayAccount(uid).FirstOrDefault();

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
                prepayRecord.RstId = Guid.Parse("CB824E58-E2CA-4C95-827A-CA62D528C6A7");
                prepayRecord.ScoreVip = 0;
                prepayRecord.SId = DateTime.Now.ToString("HHmmss") + TenPayV3Util.BuildRandomStr(28); ;
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
                billPay.Remark = "用户充值" + totalPrice + "元";
                billPay.Remove = 0;
                billPay.RstId = Guid.Parse("CB824E58-E2CA-4C95-827A-CA62D528C6A7");
                billPay.UserId = Guid.Empty;
                billPay.UserName = name;

                if (!prepayRecordModel.AddPrepayRecord(prepayRecord))
                {
                    return Content("充值数据插入失败");
                }

                if (!billPayModel.AddBillPay(billPay))
                {
                    return Content("充值数据插入失败");
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
                return Content("请求发生错误，请返回重新尝试");
            }

            var jsonData = new
            {
                appId = ViewData["appId"],
                timeStamp = ViewData["timeStamp"],
                nonceStr = ViewData["nonceStr"],
                package = ViewData["package"],
                paySign = ViewData["paySign"]
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}
