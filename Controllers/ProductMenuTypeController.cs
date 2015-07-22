using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Controllers
{
    public class ProductMenuTypeController : Controller
    {
        //
        // GET: /ProductMenuType/

        public ActionResult Index(string name, string id, string Type, string peopleCount, string orderid = null, string MemberCardNo = null)
        {
            List<ProductType> pt = null;
           // object obj = System.Web.HttpRuntime.Cache.Get("ProductType" + ProductTypeId);

           // if (obj != null)
            //{
            //    p = obj as List<ProductMenuType>;
           // }
           // if (p == null)
            //{
           // string RestaurantId = "3F5A7C2E-0B48-4738-B4D0-3848FE943913";
            ProductTypeModel ptm = new ProductTypeModel();
             pt = ptm.getProductType(id);
             //   System.Web.HttpRuntime.Cache.Add("ProductType" + ProductTypeId, p, null, DateTime.Now.AddHours(2),
             //   TimeSpan.Zero, CacheItemPriority.Normal, null);
           // }
             ViewBag.SourceAccountId = name;
             ViewBag.RestaurantId = id;
             ViewBag.Type = Type;
             ViewBag.peopleCount = peopleCount;
             ViewBag.MemberCardNo = MemberCardNo;
             ViewBag.orderid = orderid;
            return View(pt);
        }

        public ActionResult stage2(string RestaurantId, string ProductTypeId, string TypeName, string SourceAccountId, string Status, string Type = null, string imgid = null, string peopleCount=null)
        {

            List<ProductNew> p = null;
            object obj = System.Web.HttpRuntime.Cache.Get("ProductType" + ProductTypeId);

            if (obj != null)
            {
                p = obj as List<ProductNew>;
            }
            if (p == null)
            {
                ProductModel pm = new ProductModel();
                p = pm.getProductByProductTypeId(RestaurantId, ProductTypeId);
                System.Web.HttpRuntime.Cache.Add("ProductType" + ProductTypeId, p, null, DateTime.Now.AddHours(2),
                TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(SourceAccountId);
            ViewBag.MemberCardNo = crm.First().Uid;
            ViewBag.TypeName = TypeName;
            ViewBag.Type = Type;
            ViewBag.imgid = imgid;
            ViewBag.peopleCount = peopleCount;
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.RestaurantId = RestaurantId;
            /* 显示用户已点菜数量*/
            OrderModel odm = new OrderModel();
            Order FastFoodOrder = null;
            ViewBag.OrderId = null;
            FastFoodOrder = odm.SelectUnFinishedFastFoodOrder(crm.First().Uid);
            if ( FastFoodOrder != null)
            {
                if (Type == "Auto") { Type = "FastFood"; }
               
                string OrderId =FastFoodOrder.Id.ToString();
                ViewBag.OrderId = FastFoodOrder.Id;
                /* 显示用户已点菜数量*/
                MyMenuModel myMenu = new MyMenuModel();
                List<MyMenu> myAutoMenu = myMenu.getMyMenuListData(crm.First().Uid, OrderId, Type);
                ViewBag.MyAutoMenuListData = myAutoMenu;
            }
            return View(p);
        }


    }
}
