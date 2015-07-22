using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe.Models;
using System.Web.Caching;
using WitBird.XiaoChangHe.Models.Restaurant;
using WitBird.XiaoChangHe.Models.Specials;

namespace WitBird.XiaoChangHe.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/

        public ActionResult Rst(string id)
        {
            NewRestaurantAbstract p = null;
            object obj = System.Web.HttpRuntime.Cache.Get("id" + id);
            if (obj != null)
            {
                p = obj as NewRestaurantAbstract;
            }
            if (p == null)
            {
                RestaurantModel rdb = new RestaurantModel();
                p= rdb.GetRestaurentById(id);
                System.Web.HttpRuntime.Cache.Add("id" + id, p, null, DateTime.Now.AddHours(2),
                TimeSpan.Zero, CacheItemPriority.Normal, null);

                var crmMember =  Session["CrmMember"] as CrmMember;
                if (crmMember != null)
                {
                    ViewBag.MemberCardNo = crmMember.Uid;
                    ViewBag.SourceAccountId = crmMember.SourceAccountId;

                    Order FastFoodOrder = null;

                    //如果为自动点餐和快捷点餐。如果还有未过期的订单则跳转到订单详情 ViewBag.AutoOrderCount=1 有订单
                    ViewBag.AutoOrderCount = 0;
                    OrderModel odm = new OrderModel();
                    FastFoodOrder = odm.SelectUnFinishedFastFoodOrder(crmMember.Uid);
                    if (FastFoodOrder != null)
                    {
                        ViewBag.AutoOrderCount = 1;
                        ViewBag.OrderId = FastFoodOrder.Id;
                    }
                }
            }
            ViewData["special"] = SpecialsModel.GetTodayByRestaurantId(new Guid(id));
            ViewData["AllSpecial"] = SpecialsModel.GetAllByRestaurantId(new Guid(id));
            ViewData["RestaurantImages"] = RestaurantImageDBModel.GetRestaurantImages(new Guid(id));
            return View(p);
        }

        public ActionResult Message(string id, string name)
        {
            MessageModel m = new MessageModel();
           
            
            if (name.Equals("About"))
            {
                name = "E8EB7169-B824-4C5A-9D24-EE8A8B9D6206";
                
            }
            if (name.Equals("Promotion"))
            {
                ViewBag.Promotion = "";
                ViewBag.Promotion = m.getPromotionListData(id);
                ViewBag.Title = "优惠活动";
            }
            else
            {
                List<AboutUs> abts = m.getAboutUsListData(id,name);
                ViewBag.AboutUs = abts;
                
                if (abts != null && abts.Count > 0)
                {
                    ViewBag.Title = abts.First().Title;
                }
                
            }

          return View();
        }

    }
}
