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
        public ActionResult Index(string id, string name, string activityState)
        {
            int state = 1;
            if (!string.IsNullOrEmpty(activityState))
            {
                switch (activityState)
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

            ViewBag.CompanyId = id;
            ViewBag.Name = name;

            return View(list);
        }

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string id, string name, string activityId)
        {
            var intId = 0;
            if (int.TryParse(activityId, out intId))
            {
                var manager = new ActivityManager();

                var activity = manager.GetActivityDetailById(intId);

                if (activity != null)
                {
                    ViewBag.CompanyId = id;
                    ViewBag.Name = name;

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
        }

        /// <summary>
        /// 活动详情页“立即加入”之后的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Join(string id, string name)
        {
            ActionResult result = null;

            var model = new JoinActivityModel();

            var userManager = new UserManager();
            var orderManager = new OrderManager();

            Guid companyGuid = Guid.Empty;
            if (Guid.TryParse(id, out companyGuid))
            {
                var uid = userManager.GetUid(companyGuid, name);

                if (!string.IsNullOrEmpty(uid))
                {
                    ViewBag.CompanyId = id;
                    ViewBag.Name = name;
                    ViewBag.Uid = uid;

                    result = View(model);
                }
                else
                {
                    result = Redirect("/");
                }
            }
            else
            {
                result = Redirect("/");
            }

            return result;
        }

        /// <summary>
        /// 提交验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Join(string id, string name, JoinActivityModel model)
        {
            ActionResult result = null;

            var verifyCodes = new string[] { "M2J6", "N4W2", "YW45", "32KU", "L624", "8B8C", "92M2", "9P62", "C9X6", "527H", "5C32", "LP52", "5W2Q", "HK66", "67AM", "E6R3" };

            if (ModelState.IsValid)
            {
                var pass = verifyCodes.FirstOrDefault(v => v.Equals(model.VerifyCode, StringComparison.OrdinalIgnoreCase));

                if (pass != null)
                {
                    #region 在这里面去给用户加钱

                    //TODO 加钱

                    #endregion

                    //验证功过并且钱加好了之后跳转到这个页面，让用户分享
                    ViewBag.VerifyCode = pass;
                    result = View("Pass");
                }
                else//验证失败
                {
                    ViewBag.CompanyId = id;
                    ViewBag.Name = name;

                    result = View("Failed");
                }
            }
            else
            {
                ViewBag.CompanyId = id;
                ViewBag.Name = name;

                result = View("Failed");
            }

            return result;
        }
    }
}
