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
    public class SelTableInfoController : Controller
    {
        //
        // GET: /SelTableInfo/

        public ActionResult SelTableInfo(string SourceAccountId, string RestaurantId, string Status = null,string type=null)
        {
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.RestaurantId = RestaurantId;
            ViewBag.Status = Status;
            ViewBag.Type = type;
            List<SelTableInfo> p = null;
           // object obj = System.Web.HttpRuntime.Cache.Get("RestaurantId" + RestaurantId);
           // if (obj != null)
           // {
             //   p = obj as List<SelTableInfo>;
            //}
            //if (p == null)
            //{
                SelTableInfoModel info = new SelTableInfoModel();
                p = info.getTableInfo(RestaurantId);
              //  System.Web.HttpRuntime.Cache.Add("RestaurantId" + RestaurantId, p, null, DateTime.Now.AddHours(2),
               // TimeSpan.Zero, CacheItemPriority.Normal, null);
            //}
            return View(p);
        }

    }
}
