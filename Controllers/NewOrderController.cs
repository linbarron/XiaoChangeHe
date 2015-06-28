using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangeHe.Core;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;

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

        public ActionResult Detail(string id, string name, string orderId)
        {
            var orderManager = new OrderManager();

            Guid orderGuid = Guid.Empty;
            if (Guid.TryParse(orderId, out orderGuid))
            {
                var detail = orderManager.GetOrderDetailById(orderGuid);

                return View(detail);
            }
            else
            {
                return Redirect("/");
            }
        }


        /// <summary>
        /// 申请退款页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MoneyBack(string id, string name, string orderId)
        {
            ActionResult result = null;

            var orderManager = new OrderManager();

            Guid orderGuid = Guid.Empty;
            if (Guid.TryParse(orderId, out orderGuid))
            {
                var summary = orderManager.GetOrderSummary(orderGuid);

                if (summary == null)
                {
                    result = Redirect("/");
                }
                else
                {
                    result = View(summary);
                }
            }
            else
            {
                result = Redirect("/");
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">CompanyId</param>
        /// <param name="name">SourceAccountId</param>
        /// <returns></returns>
        public ActionResult My(string id, string name)
        {
            ActionResult result = null;

            var userManager = new UserManager();
            var orderManager = new OrderManager();

            Guid companyGuid = Guid.Empty;
            if (Guid.TryParse(id, out companyGuid))
            {
                var uid = userManager.GetUid(companyGuid, name);
                if (!string.IsNullOrEmpty(uid))
                {
                    var list = orderManager.GetUserOrders(uid);

                    result = View(list);
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

            ViewBag.CompanyId = id;
            ViewBag.SourceAccountId = name;

            return result;
        }
    }
}
