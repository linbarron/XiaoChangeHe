using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangeHe.Core;

namespace WitBird.XiaoChangHe.Controllers
{
    public class ActivityController : Controller
    {
        /// <summary>
        /// 活动列表
        /// </summary>
        /// <param name="id">1.进行中的活动。2.已经结束的活动</param>
        /// <returns></returns>
        public ActionResult Index(string id)
        {
            int state = 1;
            if (!string.IsNullOrEmpty(id))
            {
                switch (id)
                {
                    case "2":
                        state = 2;
                        break;
                    case "1":
                    default:
                        state = 1;
                        break;
                }
            }

            var manager = new ActivityManager();

            var list = manager.GetActivityList(state);

            ViewBag.State = state;

            return View(list);
        }

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            var intId = 0;
            if (int.TryParse(id, out intId))
            {
                var manager = new ActivityManager();

                var activity = manager.GetActivityDetailById(intId);

                if (activity != null)
                {
                    return View(activity);
                }
                else
                {
                    return Redirect("/");
                }
            }
            else
            {
                return Redirect("/");
            }

            return View();
        }
    }
}
