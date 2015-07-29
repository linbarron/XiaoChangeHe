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
                    Order order = null;
                    Order FastFoodOrder = null;
                    //Type == "FastFood"表明此店为快餐店。
                    if (!string.IsNullOrEmpty(Type) && Type == "FastFood")
                    {

                        FastFoodOrder = odm.SelectUnFinishedFastFoodOrder(crmMember.Uid);
                    }
                    else
                    {
                        order = odm.SelectUnFinishedOrder(crmMember.Uid);
                    }

                    string OrderId = "";
                    if (order != null || FastFoodOrder != null)
                    {

                        OrderId = (order != null ? order.Id.ToString() : FastFoodOrder.Id.ToString());
                        ViewBag.OrderId = OrderId;
                        /* 显示用户已点菜数量*/
                        MyMenuModel myMenu = new MyMenuModel();
                        List<MyMenu> mymenu = myMenu.getMyMenuListData(crmMember.Uid, OrderId, Type);
                        ViewBag.MyMenuListData = mymenu;
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                //TODO
                Logger.Log(ex);
            }
            ViewBag.Status = Status;
            return PartialView(model);
        }

    }
}
