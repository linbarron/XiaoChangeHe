using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WitBird.XiaoChangHe.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    public class NewOrderController : Controller
    {
        /// <summary>
        /// 订单详情（已付款）页面
        /// </summary>
        /// <returns></returns>

        public ActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// 申请退款页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MoneyBack()
        {
            return View();
        }

        /// <summary>
        /// 我的订单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult My()
        {
            return View();
        }
    }
}
