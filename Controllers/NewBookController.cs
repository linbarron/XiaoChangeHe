using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WitBird.XiaoChangHe.Controllers
{
    /// <summary>
    /// 预定
    /// </summary>
    public class NewBookController : Controller
    {
        /// <summary>
        /// 预定信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            return View();
        }
    }
}
