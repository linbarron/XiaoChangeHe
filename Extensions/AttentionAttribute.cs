using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangeHe.Core;
using WitBird.XiaoChangHe.Models;

namespace WitBird.XiaoChangHe.Extensions
{
    public class AttentionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var fromUserNameCookie = filterContext.RequestContext.HttpContext.Request.Cookies["FromUserName"];

            if (fromUserNameCookie == null || string.IsNullOrEmpty(fromUserNameCookie.Value))
            {
                filterContext.Result = new RedirectResult("http://mp.weixin.qq.com/s?__biz=MzIwMDAyNzMyNQ==&mid=210058631&idx=1&sn=c08e417ead02a0eac025ec90714f1c73#rd");
            }
            else
            {
                var userManager = new UserManager();

                var uid = userManager.GetUid(Constants.CompanyId, fromUserNameCookie.Value);

                if (string.IsNullOrEmpty(uid))
                {
                    filterContext.Result = new RedirectResult("http://mp.weixin.qq.com/s?__biz=MzIwMDAyNzMyNQ==&mid=210058631&idx=1&sn=c08e417ead02a0eac025ec90714f1c73#rd");
                }
            }
        }
    }
}