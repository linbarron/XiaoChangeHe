using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using System.Web.Caching;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Hosting;
namespace WitBird.XiaoChangHe.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/

        //public ActionResult Index(string SourceAccountId, string RestaurantId, string Status = null, string Date = null, string Time = null, string Type = null)
        //{
        //    if (Date != null & Time != null)
        //    {
        //        if (Time.Equals("0"))
        //        {
        //            DateTime data = Convert.ToDateTime(Date);
        //            data = data.AddHours(9);
        //            ViewBag.BookTime = data;
        //        }
        //        else if (Time.Equals("1"))
        //        {
        //            DateTime data = Convert.ToDateTime(Date);
        //            data = data.AddHours(16);
        //            ViewBag.BookTime = data;
        //        }
        //    }
            
        //    OrderModel odb = new OrderModel();
        //    ViewBag.ProductListData = odb.getProductListInfoData(RestaurantId);
        //    Session["begindm"] = RestaurantId;
        //    ViewBag.SourceAccountId = SourceAccountId;
        //    ViewBag.RestaurantId = RestaurantId;
        //    ViewBag.Status = Status;
        //    ViewBag.RstType = Type;
        //    CrmMemberModel cdb = new CrmMemberModel();
        //    if (SourceAccountId != null && SourceAccountId != "")
        //    {
        //        List<CrmMember> crm = cdb.getCrmMemberListInfoData(SourceAccountId);
        //        ViewBag.MemberCardNo = crm.First().Uid;
        //        string MemberCardNo = crm.First().Uid;
        //        OrderModel odm = new OrderModel();
        //        List<Order> order = null;
        //        List<FastFoodOrder> FastFoodOrder = null;
        //        if (!string.IsNullOrEmpty(Type) && Type == "FastFood")
        //        {
        //            FastFoodOrder = odm.selOrderByMemberId(crm.First().Uid);
        //        }
        //        else { order = odm.SelUnFinValidOrder(crm.First().Uid); }
               
        //        string OrderId = "";
        //        if ((order != null && order.Count > 0) || (FastFoodOrder != null && FastFoodOrder.Count > 0))
        //        {
        //            ViewBag.OrderId = order.First().Id;
        //            OrderId = order.First().Id.ToString();
        //            /* 显示用户已点菜数量*/
        //            MyMenuModel myMenu = new MyMenuModel();
        //            List<MyMenu> mymenu = myMenu.getMyMenuListData(MemberCardNo, OrderId);
        //            ViewBag.MyMenuListData = mymenu;
        //        }
               
               
        //    }
           
           
        //    return View();
        //}
        public ActionResult Begin(string id, string name, string type = null, string CityId = null, string CityName = null)
        {
            Session["CompanyId"] = id;
            ViewBag.CompanyId = id;
            OrderModel odm = new OrderModel();
            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(name);
            ViewBag.MemberCardNo = crm.First().Uid;

            Session["CrmMember"] = crm.First();

          //  Session["MemberCardNo"] = crm.First().Uid;
            if (!string.IsNullOrEmpty(CityName)) { ViewBag.cityName = CityName; }

            //根据type来判断是否是快捷预定(Quick)或智能点餐（Auto）
            if (string.IsNullOrEmpty(type) && type != "Quick" && type!= "Auto")
            {
                //判断该用户在该店面预定的时间是否小于当前时间，如果是，弹出“我的订单”选择“修改”
                //或者“新预定”框体。修改默认显示当前订单内容   如果订单是未完成状态则直接跳转到点餐页面
               
              //  ViewBag.CompanyId = id;
              
                Order info = new Order();
                List<Order> unFinOrder = odm.SelUnFinValidOrder(crm.First().Uid);
                int status = 0;
                if (unFinOrder.Count > 0)
                {
                    for (int i = 0; i < unFinOrder.Count; i++)
                    {
                        DateTime bookTime = unFinOrder[i].DiningDate;

                        if (DateTime.Now < bookTime.AddHours(5))
                        {
                            ViewBag.RecOrder = 1;
                            status = 1;
                            break;
                            //return RedirectToAction("MY", "Order", new { id = id, name = name });
                        }

                    }
                }
                if (status != 1)
                {
                    List<Order> order = odm.selOrderId(crm.First().Uid);
                    ViewBag.isCurTime = 0;
                    if (order.Count > 0)
                    {
                        for (int i = 0; i < order.Count; i++)
                        {
                            DateTime bookTime = order[i].DiningDate;

                            if (DateTime.Now < bookTime)
                            {
                                ViewBag.isCurTime = 1;
                                break;
                                //return RedirectToAction("MY", "Order", new { id = id, name = name });
                            }

                        }
                    }
                }
            }
            List<FastFoodOrder> FastFoodOrder = null;

            //如果为自动点餐和快捷点餐。如果还有未过期的订单则跳转到订单详情 ViewBag.AutoOrderCount=1 有订单
             ViewBag.AutoOrderCount=0;
            if (!string.IsNullOrEmpty(type) && type == "Auto"||type=="Quick") {

                FastFoodOrder = odm.selOrderByMemberId(crm.First().Uid);
                if (FastFoodOrder.Count > 0)
                {
                    ViewBag.AutoOrderCount = 1;
                    ViewBag.OrderId = FastFoodOrder.First().Id;
                }
            }
           
            
            
            string dm = Session["begindm"] != null ? Session["begindm"].ToString() : "";
            // if (string.IsNullOrEmpty(dm))
            //  {

          //  List<RestaurantAbstract> p = null;
           // object obj = System.Web.HttpRuntime.Cache.Get("id" + id);
           // if (obj != null)
           // {
             //   p = obj as List<RestaurantAbstract>;
           // }
           // if (p == null)
           // {
                RestaurantModel rdb = new RestaurantModel();
               CityId= string.IsNullOrEmpty(CityId) ? "510100": CityId ;
                List<RestaurantAbstract> p = rdb.getRestaurentState(id, CityId);//rdb.getRestaurantListInfoData(id);
               // System.Web.HttpRuntime.Cache.Add("id" + id, p, null, DateTime.Now.AddHours(2),
               // TimeSpan.Zero, CacheItemPriority.Normal, null);
          //  }
            ViewBag.SourceAccountId = name;
            ViewBag.type = type;
            return View(p);
            // }
            //  return RedirectToAction("Index","Order", new { SourceAccountId = id, RestaurantId = dm });
        }

        public string getCity(string lat=null,string log=null )
        {
            WebClient client = new WebClient();
            client.Encoding = UTF8Encoding.UTF8;
            Byte[] json = client.DownloadData("http://api.map.baidu.com/geocoder?location="+lat+","+log+"&output=json&key=99nS1krBR1GsM3pm7pnuxUuk");
            string a = System.Text.Encoding.UTF8.GetString(json);
            return a;

          
        }
        public int getTableTotal(string id) {
            int status=0;
            SelTableInfoModel info1 = new SelTableInfoModel();
            List<SelTableCount> data = info1.SelTableInfo(id);
            if (data.Count > 0)
               // ViewBag.total = data;
                if (data.First().total > 0) {
                    status = 1;
                }
            return status;
        }

        public FileContentResult GetBigImages(string id)
        {
             OrderModel odb = new OrderModel();
            if (!string.IsNullOrEmpty(id))
            {
               
                List<Product> list = odb.getImageProductListInfoData(id);
                if (list != null && list.Count > 0)
                {
                    Product info = list.First();
                    if (info != null )
                    {
                        if (info.OriginalImage != null)
                        {
                            return File(info.OriginalImage, "jpg", "image_" + id + ".jpg");
                        }
                        

                    }
                   
                }
            }
            List<ReceiveOrder1> list1 = odb.getDefauleImage(id);
            if (list1 != null && list1.Count > 0)
            {
                ReceiveOrder1 info = list1.First();
                if (info != null)
                {
                    if (info.DefaultImg != null)
                    {
                        return File(info.DefaultImg, "jpg", "image_" + id + ".jpg");
                    }
                }
            }

            return File(new Byte[] { }, "jpg", "image_" + id + ".jpg");
        }

        //public byte[] getByte()
        //{
        //    string path = Server.MapPath("~/Content/images/xchlogo.jpg");
        //    FileStream fs = new FileStream(path, FileMode.Open);
        //    int streamLength = (int)fs.Length;
        //    byte[] image = new byte[streamLength];
        //    fs.Read(image, 0, streamLength);
        //    fs.Close();
        //    return image;

        //}
        public FileContentResult GetImages(string id)
        {
            OrderModel odb = new OrderModel();
            if (!string.IsNullOrEmpty(id))
            {
               
                List<Product> list = odb.getBigImageProductListInfoData(id);
                if (list != null && list.Count > 0)
                {
                    Product info = list.First();
                    if (info != null)
                    {
                        if (info.ThumbImage != null)
                        {
                            return File(info.ThumbImage, "jpg", "image_" + id + ".jpg");
                        }


                    }

                }
            }
            List<ReceiveOrder1> list1 = odb.getDefauleImage(id);
            if (list1 != null && list1.Count > 0)
            {
                ReceiveOrder1 info = list1.First();
                if (info != null)
                {
                    if (info.DefaultImg != null)
                    {
                        return File(info.DefaultImg, "jpg", "image_" + id + ".jpg");
                    }
                }
            }

            return File(new Byte[] { }, "jpg", "image_" + id + ".jpg");
        }

        public FileContentResult GetImagesRst(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
               RestaurantModel rm=new RestaurantModel();
                List<RestaurantAbstract> list = rm.getImageRst(id);
                if (list != null && list.Count > 0)
                {
                    RestaurantAbstract info = list.First();
                    if (info != null)
                    {
                        if (info.Photo != null)
                        {
                            return File(info.Photo, "jpg", "image_" + id + ".jpg");
                        }


                    }

                }
            }
            return File(new byte[] {}, "jpg", "image_" + id + ".jpg");
        }
 //       public ActionResult Quick(string id, string name) {
 //           Guid OrderId = this.SaveOrders("Insert", name);
 //              CrmMemberModel cdb = new CrmMemberModel();
 //           List<CrmMember> crm = cdb.getCrmMemberListInfoData(name);
 //           if (OrderId != new Guid("00000000-0000-0000-0000-000000000000")){
 //// /book/book?MemberCardNo=@(ViewBag.MemberCardNo)&OrderId=@(ViewBag.OrderId)&SourceAccountId=@(ViewBag.SourceAccountId)
 //             return RedirectToAction("book","book", new { MemberCardNo = crm.First().Uid, OrderId = OrderId,SourceAccountId=name });

 //           }
 //               return View();
 //       }

        public Guid SaveOrders(string type, string SourceAccountId,string bookTime=null,string isQuick=null,string peopleCont=null,string Remark=null,string OrderId=null)
        {
            int i;
            OrderModel odm = new OrderModel();
            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(SourceAccountId);
            List<Order> order = null;
            List<FastFoodOrder> FastFoodOrder = null;
            Order info = new Order();
           
            //isQuick=="Quick" 表明此订单为快捷预定  isQuick==null表明为预定订单  isQuick=Auto 表明是智能点餐
            if (string.IsNullOrEmpty(isQuick) )
            {

                order = odm.SelUnFinValidOrder(crm.First().Uid);
            }
            //isQuick == "FastFood"表明此店为快餐店。
            if (!string.IsNullOrEmpty(isQuick) && isQuick == "FastFood" || isQuick == "Auto")
            {

                FastFoodOrder = odm.selOrderByMemberId(crm.First().Uid);
            }

            if ((order != null && order.Count > 0))
            {
                type = "Update";
               //  i = odm.SaveOrders(type, info);

                return order.First().Id;

            }
            if ((FastFoodOrder != null && FastFoodOrder.Count > 0)) {
              
                type = "UpdateFastFood";
                info.CreateDate = DateTime.Now;
                info.Id = new Guid(OrderId);
                info.Remark = Remark;
                i = odm.SaveOrders(type, info); 
                return FastFoodOrder.First().Id;
            }

        //if ((order!=null&&order.Count > 0)||(FastFoodOrder != null && FastFoodOrder.Count > 0))
            //{
            //    type = "Update";
            //   // i = odm.SaveOrders(type, info);

        //    return order.First().Id;

        //}
            else
            {
                info.Id = Guid.NewGuid();
                //info.Id = crm.First().Uid;
                info.ContactName = string.IsNullOrEmpty(crm.First().MemberName) ? "" : crm.First().MemberName;
                info.ContactPhone = crm.First().Tel;
                info.MemberCardNo = crm.First().Uid;
                info.OperatorId = Guid.NewGuid();//new Guid("6E4DE867-4AF0-2DEA-FC5C-036EB8637CAB")
                info.OperatorName = "xxx";
                info.CreateDate = DateTime.Now;
                if (!string.IsNullOrEmpty(isQuick) && isQuick == "FastFood")
                {
                    info.DiningDate = string.IsNullOrEmpty(bookTime) ? DateTime.Now : Convert.ToDateTime(bookTime);
                    //if (!string.IsNullOrEmpty(bookTime))
                    //{
                    //    info.DiningDate = Convert.ToDateTime(bookTime);
                    //}
                    //else {
                    //    info.DiningDate = Convert.ToDateTime("1997-10-1");
                    //}
                }
                else if (!string.IsNullOrEmpty(isQuick) && isQuick == "Auto")
                {
                    info.PersonCount = Convert.ToInt32(peopleCont);
                    info.DiningDate = DateTime.Now;
                    //info.DiningDate = string.IsNullOrEmpty(bookTime) ? DateTime.Now : Convert.ToDateTime(bookTime);

                }
                else
                {
                    info.DiningDate = string.IsNullOrEmpty(bookTime) ? DateTime.Now : Convert.ToDateTime(bookTime);

                }

                info.Status = false;
                info.ReserveType = "01";
                string RestaurantId = Session["begindm"] != null ? Session["begindm"].ToString() : "";
                info.RstId = new Guid(RestaurantId);
                i = odm.SaveOrders(type, info);
                //ViewBag.MemberCardNo = info.MemberCardNo;
                //ViewBag.OrderId = info.Id;
            }
            if (i == 1)
            {
              
                return info.Id;
            }

            return new Guid("00000000-0000-0000-0000-000000000000");

        }


        public int SaveOrderDetails(string type, string productId, string unitPrice, string orderId, string productCount, string useStatus = null, string MemberCardNo = null, string RstType=null)
        {
            int i;
            OrderDetailsModel odm = new OrderDetailsModel();
            OrderDetails info = new OrderDetails();
            MyMenuModel odb = new MyMenuModel();
            List<OrderDetails> orderD = odm.getOrderDetailInfoData(productId, orderId);
            List<MyOrderDetail> detail = null;
            if (MemberCardNo != null)
            {
               detail = odb.getMyOrderDetailListData(MemberCardNo, orderId, RstType);
            }
            if (orderD.Count > 0 && useStatus!="04")
            {
                type = "Update";
                info.ProductCount = Convert.ToInt32(productCount);
                info.CreateDate = DateTime.Now;
                info.ProductId = new Guid(productId);
                info.OrderId = new Guid(orderId);
                info.TotalPrice = Convert.ToDecimal(unitPrice) * Convert.ToInt32(productCount);                  
            }else {
                if (useStatus == "04")
                {
                    if (detail != null && detail.Count > 0)
                    {
                        foreach (MyOrderDetail item1 in detail)
                        {
                            if (item1.UseState == "04")
                            {
                                //已经有赠送的菜。（先删除已有的赠送菜，再添加现选择有赠送菜）
                                int j = odm.DelGiftOrderDetails(useStatus, orderId);
                            }
                        }
                    }
                }
                info.DetailsId = Guid.NewGuid();
                info.OrderId = new Guid(orderId);
                info.ProductId = new Guid(productId);
                info.UnitPrice = Convert.ToDecimal(unitPrice);
                info.TotalPrice = Convert.ToDecimal(unitPrice) * Convert.ToInt32(productCount);
                info.CreateDate = DateTime.Now;
                info.ProductCount = Convert.ToInt32(productCount);
                if (useStatus != "04") { info.UseState = "00"; } else { info.UseState = useStatus; }
              
                
            }
            i = odm.SaveOrderDetails(type, info);
            return i;
           
        }

        public int DelOrderDetails(string productId, string orderId)
        {

            OrderDetailsModel odm = new OrderDetailsModel();
            return odm.DelOrderDetails(productId, orderId);
        }


        public ActionResult My(string id,string name)
        {
            ViewBag.ComypanyId = id;
            ViewBag.SourceAccountId = name;
            OrderModel odm = new OrderModel();
            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(name);
            //Order info = new Order();
            //List<Order> order = odm.selOrderId(crm.First().Uid);
            //List<FastFoodOrder> Quickorder = odm.selOrderByMemberId(crm.First().Uid);
            //string OrderId = "", QuickorderId="";
            //if (order.Count > 0 || Quickorder.Count>0)
            //{

            //    ViewBag.OrderId = order.First().Id;
            //    OrderId = order.First().Id.ToString();
            //    ViewBag.QuickorderId = order.First().Id;
            //    QuickorderId = order.First().Id.ToString();
            //    if (OrderId == null || OrderId == "" || QuickorderId == null || QuickorderId == "")
            //    {
            //        ViewBag.MyOrderListData = null;
            //        return View("myOrder");

            //    }
                string MemberCardNo = crm.First().Uid;
                ViewBag.MemberCardNo = crm.First().Uid;

                MyMenuModel odb = new MyMenuModel();

                List<MyOrder> myOrder = odb.getMyOrderListData(MemberCardNo);
                List<MyOrder> myQuickOrder = odb.getMyOrderListData(MemberCardNo, "FastFood");
                if (myQuickOrder != null && myQuickOrder.Count > 0)
                {
                    ViewBag.MyQuickOrderListData = myQuickOrder;
                }
                if (myOrder != null && myOrder.Count > 0)
                {
                    ViewBag.MyOrderListData = myOrder;
                }
                //decimal sum = 0;
                //if (myOrder.Count > 0)
                //{
                //    for (int i = 0; i < myOrder.Count; i++)
                //    {
                //        sum += myOrder[i].UnitPrice*myOrder[i].;
                //    }
                //}
                //ViewBag.total = sum;
          //  }


            return View("myOrder");
        }


        public ActionResult Quick(string id, string name) {

            ViewBag.CompanyId = id;
            ViewBag.SourceAccountId = name;
            return RedirectToAction("Begin", "Order", new { id = id, name = name ,type="Quick"});
           // return View();
        }
        public ActionResult QuickOrder(string SourceAccountId, string RestaurantId,  string Date = null, string Time = null)
        {
            string BookTime="";
            if (Time.Equals("0"))
            {
                DateTime data = Convert.ToDateTime(Date);
                data = data.AddHours(9);
                BookTime = data.ToString();
            }
            else if (Time.Equals("1"))
            {
                DateTime data = Convert.ToDateTime(Date);
                data = data.AddHours(16);
                BookTime =data.ToString();
            }
            Session["begindm"] = RestaurantId;
            Guid OrderId = this.SaveOrders("Insert", SourceAccountId, BookTime, "Quick");
            ViewBag.isQuick = "Quick";
            CrmMemberModel cdb = new CrmMemberModel();
            List<CrmMember> crm = cdb.getCrmMemberListInfoData(SourceAccountId);
            if (OrderId != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                // /book/book?MemberCardNo=@(ViewBag.MemberCardNo)&OrderId=@(ViewBag.OrderId)&SourceAccountId=@(ViewBag.SourceAccountId)
                return RedirectToAction("book", "book", new { MemberCardNo = crm.First().Uid, OrderId = OrderId, SourceAccountId = SourceAccountId });

            }
            return View();
        }


         ///<summary>
        ///显示省、城市
        ///</summary>
        ///<param name="Model"></param>
        /// <returns></returns>
        public string Get_Province_By_Province(string id=null)
        {
            Province p = new Province();
            ProvinceModel model = new ProvinceModel();
            if (string.IsNullOrEmpty(id))
            {
                p.ParentId = "###";
               
            }
            else {
                p.ParentId = id;
               
            }
            p.IsUse = true;
            return model.Get_Province_By_Province(p,id);
        }


        ///<summary>
        ///根据城市名获取城市id
        ///</summary>
        ///<param name="Model"></param>
        /// <returns></returns>
        public string  getCityIdByCityName(string id = null)
        {
            ProvinceModel model = new ProvinceModel();
            List<Province> List=model.getCityIdByCityName(id);
            if(List.Count>0){
                string id1=List.First().Id.ToString();
                return id1;
            }
            return "510100";
        }



        ///<summary>
        ///优惠活动
        ///</summary>
        ///<param name="Model"></param>
        /// <returns></returns>
        public ActionResult Promotions(string id)
        {
            ViewBag.Rstid = id.ToUpper();
            return View();
        }

      

    }
}
