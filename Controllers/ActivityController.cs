using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangeHe.Core;
using WitBird.XiaoChangHe.Models;

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

        /// <summary>
        /// 活动详情页“立即加入”之后的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Join()
        {
            return View();
        }

        /// <summary>
        /// 提交验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Join(JoinActivityModel model)
        {
            ActionResult result = null;

            var verifyCodes = new string[] { "M2J6", "N4W2", "YW45", "32KU", "L624", "8B8C", "92M2", "9P62", "C9X6", "527H", "5C32", "LP52", "5W2Q", "HK66", "67AM", "E6R3" };

            if (ModelState.IsValid)
            {
                var pass = verifyCodes.Any(v => v.Equals(model.VerifyCode, StringComparison.OrdinalIgnoreCase));

                if (pass)
                {
                    #region 在这里面去给用户加钱

                    //TODO 加钱

                    #endregion

                    //验证功过并且钱加好了之后跳转到这个页面，让用户分享
                    result = View("Pass");
                }
                else//验证失败
                {
                    result = View("Failed");
                }
            }
            else
            {
                result = View("Failed");
            }

            return result;
        }
    }
}
