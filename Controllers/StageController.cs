using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Controllers
{
    public class StageController : Controller
    {
        //
        // GET: /Stage/

        public ActionResult Index(string id, string name)
        {
            ViewBag.SourceAccountId = name;
            ViewBag.CompanyId = id;
            Session["CompanyId"] = id;
            ProductModel pm = new ProductModel();
           string RestaurantId = "3F5A7C2E-0B48-4738-B4D0-3848FE943913";
           
            List<RandomProduct> list = pm.getRandomProduct(id,"1");
            Session["begindm"] = RestaurantId;
            return View(list);
        }


        public JsonResult getRandomProduct(string RestaurantId ,string peopleCount )
        {
            ProductModel pm = new ProductModel();
           // string CompanyId = Session["CompanyId"] != null ? Session["CompanyId"].ToString() : "";
            List<RandomProduct> list = pm.getRandomProduct(RestaurantId, peopleCount);
           
            JsonResult json = new JsonResult
            {
                Data = list
            };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许get访问，否则报错
            return json;

        } 
        public ActionResult Index1(string id, string name,string peopleCount,string Type,string orderid=null,string MemberCardNo=null)
        {
            ViewBag.SourceAccountId = name;
            ViewBag.RestaurantId = id;
            ViewBag.Type = Type;
            ViewBag.peopleCount = peopleCount;
            ViewBag.MemberCardNo = MemberCardNo;
            ViewBag.orderid=orderid;
          //  Session["CompanyId"] = id;

            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(name);
            ViewBag.MemberCardNo = crm.First().Uid;
            AutoMenusModel am = new AutoMenusModel();
            string RestaurantId = id;

            List<AutoMenuAndProduct> list = am.getAutoProduct(RestaurantId, peopleCount);
            Session["begindm"] = RestaurantId;
            return View(list);
        
        }



        public ActionResult Index2(string id, string name, string peopleCount, string Type, string CompanyId = null, string RstType = null, string IsOrder=null)
        {
            ViewBag.SourceAccountId = name;
            ViewBag.RestaurantId = id;
            ViewBag.Type = Type;
            ViewBag.peopleCount = peopleCount;
            ViewBag.CompanyId = CompanyId;
            //  Session["CompanyId"] = id;
            ViewBag.RstType = RstType;
            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(name);
            ViewBag.MemberCardNo = crm.First().Uid;
            AutoMenusModel am = new AutoMenusModel();
            string RestaurantId = id;
            //判断此用户有没有有效的订单。如果有则显示有订单的菜。
            Order FastFoodOrder = null;
            OrderModel odm = new OrderModel();
                FastFoodOrder = odm.SelectUnFinishedFastFoodOrder(crm.First().Uid);
                if (FastFoodOrder != null)
                {
                    //ViewBag.OrderId = FastFoodOrder.First().Id;
                   // return View(FastFoodOrder);
                    ViewBag.Orders = FastFoodOrder;
                }
            


            List<AutoMenuAndProduct> list = am.getAutoProduct(RestaurantId, peopleCount);
            Session["begindm"] = RestaurantId;
            return View(list);

        }


        public ActionResult Index3(string id, string name, string peopleCount, string Type)
        {
            ViewBag.SourceAccountId = name;
            ViewBag.RestaurantId = id;
            ViewBag.Type = Type;
            ViewBag.peopleCount = peopleCount;
            //  Session["CompanyId"] = id;

            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(name);
            ViewBag.MemberCardNo = crm.First().Uid;
            AutoMenusModel am = new AutoMenusModel();
            string RestaurantId = id;

            List<AutoMenuAndProduct> list = am.getAutoProduct(RestaurantId, peopleCount);
            Session["begindm"] = RestaurantId;
            return View(list);

        }


        public JsonResult getNextAutoProducts(string CompanyId,string peopleCount)
        {
            ProductModel pm = new ProductModel();
           // string CompanyId = Session["CompanyId"] != null ? Session["CompanyId"].ToString() : "";
            List<AutoProducts> list = pm.getAutoRandomProducts(CompanyId, peopleCount);
            ViewBag.ProductCount = list.Count();
            JsonResult json = new JsonResult
            {
                Data = list
            };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许get访问，否则报错
            return json;

        }
    }
     
}
