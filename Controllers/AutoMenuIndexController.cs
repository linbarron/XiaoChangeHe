using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Controllers
{
    public class AutoMenuIndexController : Controller
    {
        //
        // GET: /AutoMenuIndex/

        public ActionResult AutoMenuIndex(string id, string name, string Type, string CompanyId, string RstType, string IsOrder = null, string IsNewOrder = null, string orderId = null, string MemberCardNo=null)
        {
           // Session["CompanyId"] = id;
            ViewBag.RestaurantId = id;
            ViewBag.SourceAccountId = name;
            ViewBag.Type = Type;
            ViewBag.CompanyId = CompanyId;
            ViewBag.RstType = RstType;
            ViewBag.IsOrder = IsOrder;
            if (!string.IsNullOrEmpty(IsNewOrder) && IsNewOrder == "NewOrder")
            {
                OrderDetailsModel odm = new OrderDetailsModel();
                OrderModel om = new OrderModel(); 
                int i;
                i = odm.EmptyOrderDetails(MemberCardNo, orderId);
                i = om.EmptyOrder(orderId);
               
            }
            return View();
        }
        public ActionResult AutoMenu()
        {
            return View();
        }



    }
}
