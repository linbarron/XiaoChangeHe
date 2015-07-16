using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangeHe.Core;
using WitBird.XiaoChangeHe.Core.Entity;
using WitBird.XiaoChangHe.Extensions;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Controllers
{
    public class ActivityController : Controller
    {
        /// <summary>
        /// 活动列表
        /// </summary>
        /// <param name="id">1.进行中的活动。2.已经结束的活动</param>
        /// <returns></returns>
        public ActionResult Index(string activityState)
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

            return View(list);
        }

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string activityId)
        {
            var intId = 0;
            if (int.TryParse(activityId, out intId))
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
        }

        /// <summary>
        /// 活动详情页“立即加入”之后的页面
        /// </summary>
        /// <returns></returns>
        [Attention]
        public ActionResult Join()
        {
            ActionResult result = null;

            var model = new JoinActivityModel();

            var userManager = new UserManager();
            var orderManager = new OrderManager();
            var crmMemberModel = new CrmMemberModel();

            var uid = userManager.GetUid(Constants.CompanyId, Request.Cookies["FromUserName"].Value);

            ViewBag.Uid = uid;
            PrepayRecord prepayRecord = crmMemberModel.HasJoinedOnlineVipGroup(uid);

            if (prepayRecord != null)
            {
                ViewBag.VerifyCode = prepayRecord.SId;

                result = View("Pass");
            }
            else
            {
                result = View(model);
            }


            return result;
        }

        private readonly static List<FamousMan> FamousManList = new List<FamousMan>
        {
            new FamousMan { Name = "只能说声真有你的", WeiboUrl = "http://weibo.com/andyhaicheng" },
            new FamousMan { Name = "我一讲你就笑", WeiboUrl = "http://weibo.com/u/2084348103" },
            new FamousMan { Name = "新浪科技", WeiboUrl = "http://weibo.com/sinatech" },
            new FamousMan { Name = "留几手", WeiboUrl = "http://weibo.com/nimui" },
            new FamousMan { Name = "ROC1013", WeiboUrl = "http://weibo.com/836897179" }
        };

        private readonly static Random random = new Random();

        /// <summary>
        /// 提交验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Attention]
        public ActionResult Join(JoinActivityModel model)
        {
            ActionResult result = null;

            try
            {
                var verifyCodes = new string[] { "M2J6", "N4W2", "YW45", "32KU", "L624", "8B8C", "92M2", "9P62", "C9X6", "527H", "5C32", "LP52", "5W2Q", "HK66", "67AM", "E6R3" };
                CrmMemberModel crmMemberModel = new CrmMemberModel();

                string uid = crmMemberModel.getCrmMemberListInfoData(Request.Cookies["FromUserName"].Value).First().Uid;

                PrepayRecord prepayRecord = crmMemberModel.HasJoinedOnlineVipGroup(uid);

                if (prepayRecord == null)
                {
                    if (ModelState.IsValid)
                    {
                        var pass = verifyCodes.FirstOrDefault(v => v.Equals(model.VerifyCode, StringComparison.OrdinalIgnoreCase));

                        if (pass != null)
                        {
                            #region 在这里面去给用户加钱

                            if (crmMemberModel.JoinOnlineVipGroup(uid, pass))
                            {
                                //验证功过并且钱加好了之后跳转到这个页面，让用户分享
                                ViewBag.VerifyCode = pass;
                                result = View("Pass");
                            }

                            #endregion
                        }
                        else//验证失败
                        {
                            var famous = FamousManList[random.Next(FamousManList.Count)];

                            result = View("Failed", famous);
                        }
                    }
                    else
                    {
                        var famous = FamousManList[random.Next(FamousManList.Count)];

                        result = View("Failed", famous);
                    }
                }
                else
                {
                    ViewBag.VerifyCode = prepayRecord.SId;

                    var famous = FamousManList[random.Next(FamousManList.Count)];

                    result = View("Failed", famous);
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result = Content("页面加载出错");
            }

            return result;
        }

    }
}
