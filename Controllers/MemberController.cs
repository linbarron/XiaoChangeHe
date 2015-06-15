using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            CrmMemberModel cdb = new CrmMemberModel();
            ViewBag.CrmMemberListData = cdb.getCrmMemberListInfoData(name);
            CrmMemberModel cdb1 = new CrmMemberModel();
            List<CrmMember> crm = cdb1.getCrmMemberListInfoData(name);
            CrmMemberScoreModel crsModel = new CrmMemberScoreModel();
            ViewBag.PrepayAccount = 0;
            ViewBag.TotalScore = 0;
            ViewBag.PresentMoney = 0;
            if (crm.Count() > 0)
            {
                decimal dec = cdb.getPrepayAccount(crm.First().Uid).First().AccountMoney;
                ViewBag.PrepayAccount = dec;
                decimal PresentMoney = cdb.getPrepayAccount(crm.First().Uid).First().PresentMoney;
                ViewBag.PresentMoney = PresentMoney;
                int TotalScore = crsModel.SelCrmMemberScoreInfo(crm.First().Uid).First().Score;
                ViewBag.TotalScore = TotalScore;
            }

            ViewBag.SourceAccountId = name;
            ViewBag.CompanyId = id;
            return View();
        }

        public ActionResult editMemberInfo(string id, string name)
        {
            List<CrmMember> p = null;
            CrmMemberModel cdb = new CrmMemberModel();
            p = cdb.getCrmMemberListInfoData(name);
            decimal dec = cdb.getPrepayAccount(p.First().Uid).First().AccountMoney;
            ViewBag.PrepayAccount = dec;
            ViewBag.SourceAccountId = name;
            ViewBag.CompanyId = id;
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

        //消费记录   
        public ActionResult ConsumptionRecords(string id, string name, string companyId)
        {
            CrmMemberModel cdb1 = new CrmMemberModel();
            List<CrmMember> crm = cdb1.getCrmMemberListInfoData(name);
            ViewBag.PrepayAccount = 0;
            if (crm.Count() > 0)
            {
                decimal dec = cdb1.getPrepayAccount(crm.First().Uid).First().AccountMoney;
                ViewBag.PrepayAccount = dec;
            }
            ViewBag.Uid = id;
            ViewBag.CompanyId = companyId;
            List<PrepayRecord> p = null;
            PrepayRecordModel cdb = new PrepayRecordModel();
            p = cdb.getConsumptionRecordsListInfoData(id);
            return View(p);
        }

        //充值记录
        public ActionResult RechargeRecord(string id, string name, string companyId)
        {
            CrmMemberModel cdb1 = new CrmMemberModel();
            List<CrmMember> crm = cdb1.getCrmMemberListInfoData(name);
            ViewBag.PrepayAccount = 0;
            if (crm.Count() > 0)
            {
                decimal dec = cdb1.getPrepayAccount(crm.First().Uid).First().AccountMoney;
                ViewBag.PrepayAccount = dec;
            }
            ViewBag.Uid = id;
            ViewBag.CompanyId = companyId;
            List<PrepayRecord> p = null;
            PrepayRecordModel cdb = new PrepayRecordModel();
            p = cdb.getRechargeRecordListInfoData(id);
            return View(p);
        }

        public ActionResult Recharge(string id, string name, string companyId)
        {
            CrmMember crmMember = new CrmMember();
            return View(crmMember);
        }

    }
}
