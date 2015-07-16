using System;
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

            return result;
        }

        [HttpPost]
        public ActionResult CancelOrder(string uid, string orderId, bool isEdit)
        {
            ActionResult result = Content("FAILED");

            try
            {
                var orderManager = new OrderManager();
                var order = orderManager.GetOrderSummary(Guid.Parse(orderId));

                if (order != null && order.Status)
                {
                    OrderModel orderModel = new OrderModel();

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        PrepayRecordModel prepayRecordModel = new PrepayRecordModel();
                        CrmMemberModel crmMemberModel = new CrmMemberModel();

                        PrepayRecord prepayRecord = null;
                        PrepayRecord newPrepayRecord = null;
                        PrepayAccount prepayAccount = null;

                        prepayAccount = crmMemberModel.GetPrepayAccount(uid);
                        prepayRecord = prepayRecordModel.GetPrepayRecordByOrderId(orderId);

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
                        newPrepayRecord.RstId = Constants.CompanyId;
                        newPrepayRecord.ScoreVip = 0;
                        newPrepayRecord.SId = orderId;
                        newPrepayRecord.Uid = uid;
                        newPrepayRecord.UserId = "System";

                        prepayAccount.AccountMoney += newPrepayRecord.AddMoney.Value;
                        newPrepayRecord.PrepayDate = DateTime.Now;
                        newPrepayRecord.AsureDate = DateTime.Now;

                        bool success = false;

                        success = success && prepayRecordModel.AddPrepayRecord(newPrepayRecord);
                        success = success && crmMemberModel.UpdatePrepayAccount(prepayAccount);

                        if (isEdit)
                        {
                            success = orderModel.UpdateOrderStatus(Guid.Parse(orderId), false);
                        }
                        else
                        {
                            OrderDetailsModel odm = new OrderDetailsModel();
                            success = success && odm.EmptyOrderDetails(uid, orderId) > 0;
                            success = success && orderModel.EmptyOrder(orderId) > 0;
                        }

                        if (success)
                        {
                            result = Content("SUCCESS");
                            scope.Complete();
                        }
                    }
                }
                else
                {
                    result = Content("SUCCESS");
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
