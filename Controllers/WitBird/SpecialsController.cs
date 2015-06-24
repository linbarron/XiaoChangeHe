using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe.Models.Specials;

namespace WitBird.XiaoChangHe.Controllers.WitBird
{
    public class SpecialsController : Controller
    {
        //
        // GET: /Specials/

        public PartialViewResult RestaurantSpecials(string RestaurantId, string ProductTypeId, string SourceAccountId, string Status, string Type = null)
        {
            RestaurantSpecialsViewModel model=new RestaurantSpecialsViewModel();
            try
            {
                model.SpecialsList = SpecialsModel.GetTodayByRestaurantId(new Guid(RestaurantId));

                var crmMember =  Session["CrmMember"] as CrmMember;
                if (crmMember != null)
                {
                    ViewBag.MemberCardNo = crmMember.Uid;
                    ViewBag.SourceAccountId = crmMember.SourceAccountId;

                    #region 原來邏輯

                    OrderModel odm = new OrderModel();
                    List<Order> order = null;
                    List<FastFoodOrder> FastFoodOrder = null;
                    //Type == "FastFood"表明此店为快餐店。
                    if (!string.IsNullOrEmpty(Type) && Type == "FastFood")
                    {

                        FastFoodOrder = odm.selOrderByMemberId(crmMember.Uid);
                    }
                    else
                    {
                        order = odm.SelUnFinValidOrder(crmMember.Uid);
                    }

                    string OrderId = "";
                    if (order != null && order.Count > 0 || FastFoodOrder != null && FastFoodOrder.Count > 0)
                    {

                        ViewBag.OrderId = order != null && order.Count > 0 ? order.First().Id : FastFoodOrder.First().Id;
                        OrderId = order != null && order.Count > 0 ? order.First().Id.ToString() : FastFoodOrder.First().Id.ToString();
                        /* 显示用户已点菜数量*/
                        MyMenuModel myMenu = new MyMenuModel();
                        List<MyMenu> mymenu = myMenu.getMyMenuListData(crmMember.Uid, OrderId, Type);
                        ViewBag.MyMenuListData = mymenu;
                    }

                    #endregion
                }

            }
            catch (Exception)
            {
                //TODO
            }
            ViewBag.Status = Status;
            return PartialView(model);
        }

    }
}
