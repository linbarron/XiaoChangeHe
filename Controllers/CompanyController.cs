using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe.Models;
using System.Web.Caching;

namespace WitBird.XiaoChangHe.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/

        public ActionResult Rst(string id,string name)
        {
            List<Restaurant> p = null;
            object obj = System.Web.HttpRuntime.Cache.Get("id" + id);
            if (obj != null)
            {
                p = obj as List<Restaurant>;
            }
            if (p == null)
            {
                RestaurantModel rdb = new RestaurantModel();
              //p= rdb.getRestaurentState(id,"1");
                System.Web.HttpRuntime.Cache.Add("id" + id, p, null, DateTime.Now.AddHours(2),
                TimeSpan.Zero, CacheItemPriority.Normal, null);
            }

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
