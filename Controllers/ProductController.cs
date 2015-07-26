using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult getProductByProductTypeId(string RestaurantId, string ProductTypeId, string SourceAccountId, string Status, string Type = null)
        {

            List<ProductNew> p = null;
            //object obj = System.Web.HttpRuntime.Cache.Get("ProductType" + ProductTypeId);

            //if (obj != null)
            //{
            //    p = obj as List<ProductNew>;
            //}
            //if (p == null)
            //{
            ProductModel pm = new ProductModel();
            p = pm.getProductByProductTypeId(RestaurantId, ProductTypeId);
            //  System.Web.HttpRuntime.Cache.Add("ProductType" + ProductTypeId, p, null, DateTime.Now.AddHours(2),
            //  TimeSpan.Zero, CacheItemPriority.Normal, null);
            //}
            CrmMemberModel cdb = new CrmMemberModel();
            if (SourceAccountId != null && SourceAccountId != "")
            {
                List<CrmMember> crm = cdb.getCrmMemberListInfoData(SourceAccountId);
                ViewBag.MemberCardNo = crm.First().Uid;
                string MemberCardNo = crm.First().Uid;
                OrderModel odm = new OrderModel();
                Order order = null;
                Order FastFoodOrder = null;
                //Type == "FastFood"表明此店为快餐店。
                if (!string.IsNullOrEmpty(Type) && Type == "FastFood")
                {

                    FastFoodOrder = odm.SelectUnFinishedFastFoodOrder(crm.First().Uid);
                }
                else
                {
                    order = odm.SelectUnFinishedOrder(crm.First().Uid);
                }

                string OrderId = "";
                if (order != null || FastFoodOrder != null)
                {
                    OrderId = order != null ? order.Id.ToString() : FastFoodOrder.Id.ToString();
                    ViewBag.OrderId = OrderId;
                    /* 显示用户已点菜数量*/
                    MyMenuModel myMenu = new MyMenuModel();
                    List<MyMenu> mymenu = myMenu.getMyMenuListData(MemberCardNo, OrderId, Type);
                    ViewBag.MyMenuListData = mymenu;
                }
            }
            ViewBag.Status = Status;
            return View(p);
        }

        public ActionResult getMemProducts(string id, string name)
        {
            ProductModel p = new ProductModel();
            ViewBag.Uid = name;
            ViewBag.CompanyId = id;
            List<MemProduct> result = p.getMemProducts(id);
            ViewBag.PrepayAccount = 0;
            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(name);
            if (crm.Count() > 0)
            {
                decimal dec = cdb.GetPrepayAccount(crm.First().Uid).AccountMoney;
                ViewBag.PrepayAccount = dec;
            }
            return View(result);

        }


        public FileContentResult getImageProductMenu(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ProductModel rm = new ProductModel();
                List<ProductMenu> list = rm.getImageProductMenu(id);
                if (list != null && list.Count > 0)
                {
                    ProductMenu info = list.First();
                    if (info != null)
                    {
                        if (info.ThumbImage != null)
                        {
                            return File(info.ThumbImage, "jpg", "image_" + id + ".jpg");
                        }


                    }

                }
            }
            return File(new byte[] { }, "jpg", "image_" + id + ".jpg");
        }


        public ActionResult GetTotalCountAndPrice(string uid, string orderId)
        {
            try
            {

                MyMenuModel myMenu = new MyMenuModel();
                List<MyMenu> mymenu = myMenu.getMyMenuListData(uid, orderId, "FastFood");

                var totalCount = 0;
                var totalPrice = 0.00m;

                if (mymenu != null)
                {
                    foreach (var item in mymenu)
                    {
                        totalCount += item.ProductCount;
                        totalPrice += item.ProductCount * item.UnitPrice;
                    }
                }

                var data = new { IsSuccessful = true, TotalCount = totalCount, TotalPrice = totalPrice };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                var data = new { IsSuccessful = false };
                return Json(data, JsonRequestBehavior.AllowGet);
            }


        }

    }
}
