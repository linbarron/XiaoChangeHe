﻿using System;
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

        public ActionResult Detail(string MemberCardNo, string OrderId, string SourceAccountId, string ComypanyId = null, string Type = null, string RstType = null)
        {
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;

            var orderManager = new OrderManager();

            Guid orderGuid = Guid.Empty;
            if (Guid.TryParse(OrderId, out orderGuid))
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
        public ActionResult MoneyBack()
        {
            return View();
        }

        /// <summary>
        /// 我的订单页面
        /// 重写OrderController/My
        /// </summary>
        /// <returns></returns>
        public ActionResult My(string id, string name)
        {
            ViewBag.CompanyId = id;
            ViewBag.SourceAccountId = name;

            OrderModel orderManager = new OrderModel();
            CrmMemberModel crmManager = new CrmMemberModel();
            MyMenuModel menuManager = new MyMenuModel();

            var members = crmManager.getCrmMemberListInfoData(name);

            if (members != null && members.Count > 0)
            {
                var currentMember = members.FirstOrDefault();
                if (currentMember != null)
                {
                    var memberCardNo = currentMember.Uid;
                    ViewBag.MemberCardNo = currentMember.Uid;

                    var myOrders = menuManager.getMyOrderListData(memberCardNo);
                    var myQuickOrders = menuManager.getMyOrderListData(memberCardNo, "FastFood");

                    if (myOrders != null && myOrders.Count > 0)
                    {
                        ViewBag.MyOrders = myOrders;
                    }
                    if (myQuickOrders != null && myQuickOrders.Count > 0)
                    {
                        ViewBag.MyQuickOrders = myQuickOrders;
                    }

                    return View();
                }
                else
                {
                    Redirect("/");
                }
            }
            else
            {
                Redirect("/");
            }

            return View();
        }
    }
}
