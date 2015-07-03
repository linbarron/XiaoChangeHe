using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe.Models.SubmitOrder;

namespace WitBird.XiaoChangHe.Controllers
{
    public class MyMenuController : Controller
    {
        //
        // GET: /MyMenu/

        public ActionResult MyMenu(string MemberCardNo, string OrderId, string SourceAccountId, string RstType = null, string ComypanyId=null)
        {

            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;
            string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.RestaurantId = RestaurantId;
            if (OrderId != null && OrderId != "") {
                MyMenuModel odb = new MyMenuModel();

                List<MyMenu> mymenu = odb.getMyMenuListData(MemberCardNo, OrderId,RstType);

                ViewBag.MyMenuListData = mymenu;
                decimal sum = 0;
                if (mymenu.Count > 0)
                {
                    for (int i = 0; i < mymenu.Count; i++)
                    {
                        sum += mymenu[i].UnitPrice*mymenu[i].ProductCount;
                    }
                }
                ViewBag.total = sum;

            }
            ViewBag.RstType = RstType;
            ViewBag.ComypanyId = ComypanyId;
          
                return View();
        }

        public ActionResult MyOrderDetail(string MemberCardNo, string OrderId, string SourceAccountId, string ComypanyId=null, string Type = null, string RstType=null)
        {
            ViewBag.SourceAccountId = SourceAccountId;
            ViewBag.OrderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;
            CrmMemberModel cdb1 = new CrmMemberModel();
            ViewBag.PrepayAccount = 0;
            decimal dec = cdb1.getPrepayAccount(MemberCardNo).First().AccountMoney;
             ViewBag.PrepayAccount = dec;
          string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            ViewBag.RestaurantId=RestaurantId;
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
        List<ReceiveOrder2> list=m.SelReceiveOrder2Info(RestaurantId);
        if (list != null && list.Count > 0) { ViewBag.Explain = list.First().Explain; }
          return View(detail);
        }

        public int EmptyMyMenu(string MemberCardNo, string orderId,string SourceAccountId,string type=null)
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

        public ActionResult MyAutoMenus(string MemberCardNo, string OrderId, string SourceAccountId, string RstType, string Type = null, string peopleCount=null)
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
                mymenu= odb.getMyMenuListData(MemberCardNo, OrderId,RstType);
                ViewBag.MyMenuListData = mymenu;
                decimal sum = 0 ,msum=0;
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
            if (presentProducts != null && presentProducts.Count > 0) {
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
                var currentUser = Session["CrmMember"] as CrmMember;
                CrmMemberModel cdb1 = new CrmMemberModel();
                decimal dec = cdb1.getPrepayAccount(currentUser.Uid).First().AccountMoney;
                if (!SubmitOrderDBModel.UpdateOrderInfo(order))
                {
                    msg = "提交订单失败！";
                }
            }
            catch (Exception ex)
            {
                msg = "出错了！";
            }
            return Json(msg);
        }
    }
}
