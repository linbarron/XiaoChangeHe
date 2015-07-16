using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;

namespace WitBird.XiaoChangHe.Controllers
{
    public class JumpController : Controller
    {
        /// <summary>
        /// 将fromUserName记录到Cookie中，然后跳转到指定页面上
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult To(string fromUserName, string url)
        {
            Response.Cookies.Add(new HttpCookie("FromUserName", fromUserName));

            var model = new JumpModel
            {
                ToUrl = url
            };

            return View(model);
        }
    }
}
