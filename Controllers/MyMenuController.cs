using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe.Models.SubmitOrder;

namespace WitBird.XiaoChangHe.Controllers
{
    public class MyMenuController : Controller
    {
        //
        // GET: /MyMenu/

        public ActionResult MyMenu(string MemberCardNo, string OrderId, string SourceAccountId, string RstType = null, string CompanyId = null)
        {

            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;
            string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.RestaurantId = RestaurantId;
            if (OrderId != null && OrderId != "")
            {
                MyMenuModel odb = new MyMenuModel();

                List<MyMenu> mymenu = odb.getMyMenuListData(MemberCardNo, OrderId, RstType);

                ViewBag.MyMenuListData = mymenu;
                decimal sum = 0;
                if (mymenu.Count > 0)
                {
                    for (int i = 0; i < mymenu.Count; i++)
                    {
                        sum += mymenu[i].UnitPrice * mymenu[i].ProductCount;
                    }
                }
                ViewBag.total = sum;

            }
            ViewBag.RstType = RstType;
            ViewBag.CompanyId = CompanyId;

            return View();
        }
        
        public int EmptyMyMenu(string MemberCardNo, string orderId, string SourceAccountId, string type = null)
        {
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = orderId;
            ViewBag.MemberCardNo = MemberCardNo;
            OrderDetailsModel odm = new OrderDetailsModel();
            OrderModel om = new OrderModel();
            //  List<OrderDetails> list = odm.getOrderDetaiById(orderId);
            int i;
            //if (list==null&&list.Count== 0)
            //{
            //    i = om.EmptyOrder(orderId);
            //}
            //else
            //{emptyOrdersDetail

            i = odm.EmptyOrderDetails(MemberCardNo, orderId);
            //  if (i == 1)
            //  {
            if (string.IsNullOrEmpty(type))
            {
                i = om.EmptyOrder(orderId);
            }
            // }
            // }
            return i;
            // return RedirectToAction("MyMenu", "MyMenu", new { MemberCardNo = MemberCardNo, OrderId = orderId, SourceAccountId = SourceAccountId });
        }

        public ActionResult MyAutoMenus(string MemberCardNo, string OrderId, string SourceAccountId, string RstType, string Type = null, string peopleCount = null)
        {
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;
            ViewBag.RstType = RstType;
            ViewBag.Type = Type;
            ViewBag.peopleCount = peopleCount;
            string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.RestaurantId = RestaurantId;
            List<MyMenu> mymenu = null;
            MyMenuModel odb = new MyMenuModel();
            if (OrderId != null && OrderId != "")
            {
                mymenu = odb.getMyMenuListData(MemberCardNo, OrderId, RstType);
                ViewBag.MyMenuListData = mymenu;
                decimal sum = 0, msum = 0;
                if (mymenu.Count > 0)
                {
                    for (int i = 0; i < mymenu.Count; i++)
                    {
                        sum += mymenu[i].UnitPrice * mymenu[i].ProductCount;
                        msum += mymenu[i].MemberPrice * mymenu[i].ProductCount;

                    }
                }
                ViewBag.total = sum;
                ViewBag.mTotal = msum;
            }
            List<PresentProduct> presentProducts = null;
            presentProducts = odb.getPresentProductListData(RestaurantId);
            ViewBag.presentProducts = null;
            if (presentProducts != null && presentProducts.Count > 0)
            {
                ViewBag.presentProducts = presentProducts;
            }

            return View(mymenu);
        }

        [HttpPost]
        public JsonResult SubmitOrder(SubmitOrderEntity order)
        {
            string msg = string.Empty;
            try
            {
                CrmMemberModel cdb1 = new CrmMemberModel();

                if (string.IsNullOrEmpty(order.SourceAccountId))
                {
                    order.SourceAccountId = Session["SourceAccountId"] as string;
                }
                var currentUser = cdb1.getCrmMemberListInfoData(order.SourceAccountId).FirstOrDefault();
                decimal dec = cdb1.GetPrepayAccount(currentUser.Uid).AccountMoney;
                if (!SubmitOrderDBModel.UpdateOrderInfo(order))
                {
                    msg = "提交订单失败！";
                }
            }
            catch (Exception ex)
            {
                msg = "提交订单失败";
                Logger.Log(LoggingLevel.Normal, ex);
            }
            return Json(msg);
        }
    }
}
