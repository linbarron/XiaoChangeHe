using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal;
using EntityModel = WitBird.XiaoChangeHe.Core.Dal.Entity;
using BusinessModel = WitBird.XiaoChangeHe.Core.Entity;

namespace WitBird.XiaoChangeHe.Core
{
    public class OrderManager
    {
        public List<BusinessModel.OrderSummary> GetUserOrders(string memberCardNo)
        {
            List<BusinessModel.OrderSummary> list = null;

            OrderDal orderDal = new OrderDal();

            var entities = orderDal.GetUserOrders(memberCardNo);
            if (entities != null && entities.Count > 0)
            {
                list = entities.Select(v => new BusinessModel.OrderSummary
                {
                    OrderId = v.OrderId,
                    RestaurantId = v.RestaurantId,
                    RestaurantName = v.RestaurantName,
                    ContactName = v.ContactName,
                    ContactPhone = v.ContactPhone,
                    TotalMoney = v.TotalMoney,
                    Status = v.Status,
                    DiningDate = v.DiningDate,
                    CreateTime = v.CreateTime,
                    Backlog = v.Backlog,
                    PersonCount = v.PersonCount
                }).ToList();
            }

            return list;
        }

        public EntityModel.OrderSummary GetOrderSummary(Guid orderId)
        {
            EntityModel.OrderSummary summary = null;

            OrderDal orderDal = new OrderDal();

            var entity = orderDal.GetOrderSummary(orderId);
            if (entity != null)
            {
                summary = new EntityModel.OrderSummary();

                //todo
            }

            return summary;
        }

        public BusinessModel.OrderDetail GetOrderDetailById(Guid orderId)
        {
            BusinessModel.OrderDetail detail = null;

            OrderDal orderDal = new OrderDal();

            var orderSummary = orderDal.GetOrderSummary(orderId);

            if (orderSummary != null)
            {
                detail = new BusinessModel.OrderDetail();

                detail.OrderId = orderSummary.OrderId;
                detail.ContactName = orderSummary.ContactName;
                detail.Telephone = orderSummary.ContactPhone;
                detail.DiningDate = orderSummary.DiningDate;
                detail.CreateTime = orderSummary.CreateTime;
                detail.Backlog = orderSummary.Backlog;
                detail.PersonCount = orderSummary.PersonCount;

                var details = orderDal.GetOrderDetails(orderId);

                if (details != null)
                {
                    var productIds = details.Select(v => v.ProductId).ToList();

                    var products = orderDal.GetProducts(productIds);

                    decimal orderTotallMoney = 0;

                    if (products != null && products.Count > 0)
                    {
                        detail.ProductList = new List<BusinessModel.Product>();

                        foreach (var subDetail in details)
                        {
                            var productDetail = products.FirstOrDefault(v => v.ProductId == subDetail.ProductId);
                            if (productDetail != null)
                            {
                                var product = new BusinessModel.Product();

                                product.ProductId = subDetail.ProductId;
                                product.UnitPrice = subDetail.UnitPrice;
                                product.Count = subDetail.ProductCount;
                                orderTotallMoney += subDetail.TotalPrice;//计算订单总金额

                                product.ProductName = productDetail.ProductName;
                                productDetail.Image = productDetail.Image;

                                detail.ProductList.Add(product);
                            }
                        }
                    }

                    detail.TotalMoney = orderTotallMoney;
                }
            }

            return detail;
        }
    }
}
