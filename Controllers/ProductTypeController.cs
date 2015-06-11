using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using System.Web.Caching;
namespace WitBird.XiaoChangHe.Controllers
{
    public class ProductTypeController : Controller
    {
        //
        // GET: /ProductType/
        public ActionResult Index(string SourceAccountId, string RestaurantId, string Status = null, string Date = null, string Time = null, string Type = null, string IsOrder = null, string MemberCardNo = null, string Orderid=null)
        {
              ProductTypeModel ptm = new ProductTypeModel();
            if (Date != null & Time != null)
            {
                if (Time.Equals("0"))
                {
                    DateTime data = Convert.ToDateTime(Date);
                    data = data.AddHours(9);
                    ViewBag.BookTime = data;
                }
                else if (Time.Equals("1"))
                {
                    DateTime data = Convert.ToDateTime(Date);
                    data = data.AddHours(16);
                    ViewBag.BookTime = data;
                }
            }
            if (!string.IsNullOrEmpty(IsOrder) && IsOrder == "1")
            {
                OrderDetailsModel odm = new OrderDetailsModel();
                OrderModel om = new OrderModel();
                int i;
                i = odm.EmptyOrderDetails(MemberCardNo, Orderid);
                i = om.EmptyOrder(Orderid);

            }
             List <ProductType> pt=null;
             if (!string.IsNullOrEmpty(RestaurantId) && RestaurantId.ToUpper() == "0E71D411-EFE0-4964-8E3D-2901E0823815")
             {
                 List<ProductType> onept = ptm.getOneProductType(RestaurantId);
                 ViewBag.FirstMenus = onept;
                if(onept!=null&&onept.Count()>0)
                 pt = ptm.getProductTypebyId(RestaurantId, onept.First().TypeId.ToString());

             }
             else
             {

                 pt = ptm.getProductType(RestaurantId);
             }
            Session["begindm"] = RestaurantId;
            string RestaurantId1 = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.Status = Status;
            ViewBag.FastFood = Type;
            return View(pt);
        }


        public ActionResult getSecondProductTypeByTypeId(string RestaurantId, string TypeId)
        {

            List<ProductType> p = null;
            object obj = System.Web.HttpRuntime.Cache.Get("SecondProductType" + TypeId);

            if (obj != null)
            {
                p = obj as List<ProductType>;
            }
            if (p == null)
            {
                ProductTypeModel ptm = new ProductTypeModel();
                p = ptm.getProductTypebyId(RestaurantId, TypeId);
                System.Web.HttpRuntime.Cache.Add("ProductType" + TypeId, p, null, DateTime.Now.AddHours(2),
                TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
           
            return View(p);
        }

    }
}
