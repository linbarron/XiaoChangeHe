using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WitBird.XiaoChangHe.Controllers
{
    public class ActivityController : Controller
    {
        /// <summary>
        /// 活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            return View();
        }
    }
}
