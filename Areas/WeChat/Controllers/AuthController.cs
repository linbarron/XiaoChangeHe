using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WitBird.XiaoChangHe.Areas.WeChat.Controllers
{
    public class AuthController : Controller
    {
        private static readonly string Token =
            System.Configuration.ConfigurationManager.AppSettings["Token"];

        /// <summary>
        /// 微信后台验证地址（使用Get）
        /// </summary>
        [HttpGet]
        public ActionResult Index(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                return Content(echostr);//返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token));
            }
        }

    }
}
