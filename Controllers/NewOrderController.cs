﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            CrmMemberModel cdb1 = new CrmMemberModel();
            ViewBag.PrepayAccount = 0;
            decimal dec = cdb1.getPrepayAccount(MemberCardNo).First().AccountMoney;
            ViewBag.PrepayAccount = dec;
            string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.RestaurantId = RestaurantId;
            MyMenuModel odb = new MyMenuModel();

            List<MyOrderDetail> detail = odb.getMyOrderDetailListData(MemberCardNo, OrderId, RstType);
            // ViewBag.MyOrderDetail = detail;

            if (OrderId != null && OrderId != "")
            {
                MyMenuModel odb1 = new MyMenuModel();

                // List<MyMenu> mymenu = odb1.getMyMenuListData(MemberCardNo, OrderId);

                // ViewBag.MyMenuListData = mymenu;
                decimal sum = 0;
                if (detail.Count > 0)
                {
                    for (int i = 0; i < detail.Count; i++)
                    {
                        if (detail[i].UnitPrice != 0)
                        {
                            sum += detail[i].MemberPrice * detail[i].ProductCount;
                        }
                    }
                }
                ViewBag.MemberPriceTotal = sum;

            }
            //获取该店面的就餐时间
            ViewBag.Explain = "";
            ReceiveOrderModel m = new ReceiveOrderModel();
            List<ReceiveOrder2> list = m.SelReceiveOrder2Info(RestaurantId);
            if (list != null && list.Count > 0) { ViewBag.Explain = list.First().Explain; }
            return View(detail);
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
