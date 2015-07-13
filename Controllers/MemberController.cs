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
            Session["SourceAccountId"] = name;
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
            Session["SourceAccountId"] = SourceAccountId;
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
                Session["SourceAccountId"] = name;
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
    }
}
