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
    public class BookController : Controller
    {
        //
        // GET: /Book/

        public ActionResult Book(string MemberCardNo, string OrderId, string SourceAccountId)
        {
            Order info = new Order();
            OrderModel odm = new OrderModel();
            //List<Order> order = odm.selOrderId(MemberCardNo);

            Guid orderId= Guid.Empty;
            if (Guid.TryParse(OrderId, out orderId))
            {
                ViewBag.orderInfo = odm.selOrderByOrderId(orderId);
            }
            ViewBag.orderId = OrderId;
            ViewBag.MemberCardNo = MemberCardNo;
            ViewBag.SourceAccountId = SourceAccountId;
            return View();
        }

        public ActionResult SaveOrders(string SourceAccountId, string MemberCardNo, string orderid, string peoCount, string seat, string name, string phone, string sex, string remark)
        {
            OrderModel odm = new OrderModel();
            Order order = new Order();
            order.Id = new Guid(orderid);
            // DateTime date1 = Convert.ToDateTime(date);
            // DateTime time1 = Convert.ToDateTime(time);
            // date1= date1.AddHours(time1.Hour);
            // date1= date1.AddMinutes(time1.Minute);
            //date1=  date1.AddSeconds(time1.Second);

            //  order.DiningDate = Convert.ToDateTime(date1);
            order.PersonCount = string.IsNullOrEmpty(peoCount) ? 0 : Convert.ToInt32(peoCount);
            //order.TableCount = Convert.ToInt32(seat);
            order.TableCount = 1;
            order.ContactName = name;
            order.ContactPhone = phone;
            order.Sex = Convert.ToBoolean(sex);
            order.Remark = remark;
            order.Status = OrderStatus.New;
            order.CreateDate = DateTime.Now;
            int i = odm.SaveOrders("Update", order);
            string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            string CompanyId = Session["CompanyId"] != null ? Session["CompanyId"].ToString() : "";
            //   return RedirectToAction("MyMenu", "MyMenu", new { MemberCardNo = MemberCardNo, OrderId = orderid ,SourceAccountId=SourceAccountId});

            return RedirectToAction("MyOrderDetail", "MyMenu", new { MemberCardNo = MemberCardNo, OrderId = orderid, SourceAccountId = SourceAccountId, CompanyId = CompanyId });
            // <a href="/pay/MyOrderDetail?MemberCardNo=@(ViewBag.MemberCardNo)&OrderId=@(item.OrderId)&SourceAccountId= @(ViewBag.SourceAccountId)&type='detail'&CompanyId=@( ViewBag.CompanyId)" style="color:#ccc">


        }

        //public ActionResult SaveOrders(string SourceAccountId, string date, string peoCount, string seat, string name, string phone, string sex, string remark)
        //{
        //    OrderModel odm = new OrderModel();
        //    Order order = new Order();
        //    order.Id = new Guid(orderid);
        //    order.DiningDate = Convert.ToDateTime(date);
        //    order.PersonCount = string.IsNullOrEmpty(peoCount) ? 0 : Convert.ToInt32(peoCount);
        //    order.TableCount = Convert.ToInt32(seat);
        //    order.ContactName = name;
        //    order.ContactPhone = phone;
        //    order.Sex = Convert.ToBoolean(sex);
        //    order.Remark = remark;
        //    order.Status = true;
        //    order.CreateDate = DateTime.Now;
        //    int i = odm.SaveOrders("Update", order);
        //    string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
        //    string CompanyId = Session["CompanyId"] != null ? Session["CompanyId"].ToString() : "";
        //    //   return RedirectToAction("MyMenu", "MyMenu", new { MemberCardNo = MemberCardNo, OrderId = orderid ,SourceAccountId=SourceAccountId});

        //    return RedirectToAction("MyOrderDetail", "MyMenu", new { MemberCardNo = MemberCardNo, OrderId = orderid, SourceAccountId = SourceAccountId, CompanyId = CompanyId });
        //    // <a href="/pay/MyOrderDetail?MemberCardNo=@(ViewBag.MemberCardNo)&OrderId=@(item.OrderId)&SourceAccountId= @(ViewBag.SourceAccountId)&type='detail'&CompanyId=@( ViewBag.CompanyId)" style="color:#ccc">


        //}


    }
}
