using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/

        public ActionResult Index(string id)
        {
            List<News> p = null;
            NewsModel info = new NewsModel();
            p = info.SelNewsInfo(id);
            return View(p);
           
        }

      

    }
}
