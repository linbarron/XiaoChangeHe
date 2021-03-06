﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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

            try
            {
                CrmMemberModel crmMemberModel = new CrmMemberModel();
                var member = crmMemberModel.getCrmMemberListInfoData(name).First();

                ViewBag.Uid = member.Uid;
                ViewBag.SourceAccountId = name;

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
            catch (Exception ex)
            {
                Logger.Log(ex);
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

            try
            {

                var userManager = new UserManager();
                var orderManager = new OrderManager();

                string uid = "";
                Guid companyGuid = Guid.Empty;
                if (Guid.TryParse(id, out companyGuid))
                {
                    uid = userManager.GetUid(companyGuid, name);
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
                ViewBag.Uid = uid;

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                result = Redirect("/");
            }
            return result;
        }

        /// <summary>
        /// 退款或者取消订单
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="orderId"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelOrder(string uid, string orderId, bool isEdit)
        {
            ActionResult result = Content("FAILED");

            try
            {
                bool success = true;
                var orderManager = new OrderManager();
                var order = orderManager.GetOrderSummary(Guid.Parse(orderId));

                if (order != null)
                {
                    OrderModel orderModel = new OrderModel();

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                        CrmMemberModel crmMemberModel = new CrmMemberModel();

                        if (isEdit)
                        {
                            if (order.Status == OrderStatus.New)
                            {
                                success = orderModel.UpdateOrderStatus(Guid.Parse(orderId), OrderStatus.Cancelled);

                                var prepayRecord = prepayRecordModel.GetPrepayRecordByOrderId(orderId);

                                if (prepayRecord != null)
                                {
                                    //取消订单，支付失败
                                    prepayRecord.AsureDate = DateTime.Now;
                                    prepayRecord.RState = "99";
                                    success = success && prepayRecordModel.UpdatePrepayRecord(prepayRecord);
                                }
                            }
                            else
                            {
                                success = false;
                            }
                        }
                        else
                        {
                            if (OrderStatus.Paid == order.Status)
                            {
                                PrepayRecord prepayRecord = null;
                                PrepayRecord newPrepayRecord = null;
                                PrepayAccount prepayAccount = null;

                                prepayAccount = crmMemberModel.GetPrepayAccount(uid);
                                prepayRecord = prepayRecordModel.GetPrepayRecordByOrderId(orderId);

                                //已退款
                                prepayRecord.RState = "02";
                                prepayRecord.AsureDate = DateTime.Now;

                                newPrepayRecord = new PrepayRecord();

                                newPrepayRecord.AddMoney = -prepayRecord.AddMoney;
                                newPrepayRecord.AsureDate = DateTime.Now;
                                newPrepayRecord.BillPayId = Guid.NewGuid();
                                newPrepayRecord.DiscountlMoeny = 0;
                                newPrepayRecord.PayByScore = 0;
                                newPrepayRecord.PayModel = "02";
                                newPrepayRecord.PrepayDate = DateTime.Now;
                                newPrepayRecord.PrepayMoney = -0;
                                newPrepayRecord.PresentMoney = 0;
                                newPrepayRecord.PromotionId = 0;
                                newPrepayRecord.RecMoney = 0;
                                newPrepayRecord.RecordId = -1;
                                newPrepayRecord.RState = "";
                                newPrepayRecord.RstId = order.RestaurantId;
                                newPrepayRecord.ScoreVip = 0;
                                newPrepayRecord.SId = "";
                                newPrepayRecord.Uid = uid;
                                newPrepayRecord.UserId = "System";

                                prepayAccount.AccountMoney += newPrepayRecord.AddMoney.Value;
                                newPrepayRecord.PrepayDate = DateTime.Now;
                                newPrepayRecord.AsureDate = DateTime.Now;

                                success = orderModel.UpdateOrderStatus(Guid.Parse(orderId), OrderStatus.Refunded);
                                success = success && prepayRecordModel.AddPrepayRecord(newPrepayRecord);
                                success = success && crmMemberModel.UpdatePrepayAccount(prepayAccount);
                                success = success && prepayRecordModel.UpdatePrepayRecord(prepayRecord);
                            }
                            else
                            {
                                success = false;
                            }
                        }

                        if (success)
                        {
                            result = Content("SUCCESS");
                            scope.Complete();
                        }
                        else
                        {
                            result = Content("FAILED");
                            scope.Dispose();
                        }
                    }
                }
                else
                {
                    result = Content("FAILED");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

            return result;
        }
    }
}
