using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal;
using WitBird.XiaoChangeHe.Core.Entity;

namespace WitBird.XiaoChangeHe.Core
{
    public class OrderManager
    {

        public OrderDetail GetOrderDetailById(Guid orderId)
        {
            OrderDetail detail = null;

            OrderDal orderDal = new OrderDal();

            var orderSummary = orderDal.GetOrderSummary(orderId);

            if (orderSummary != null)
            {
                detail = new OrderDetail();

                detail.OrderId = orderSummary.OrderId;
                detail.CustomName = orderSummary.CustomName;
                detail.Telephone = orderSummary.Telephone;
                detail.DiningDate = orderSummary.DiningDate;
                detail.CreateTime = orderSummary.CreateTime;
                detail.Backlog = orderSummary.Backlog;

                var details = orderDal.GetOrderDetails(orderId);

                if (details != null)
                {
                    var productIds = details.Select(v => v.ProductId).ToList();

                    var products = orderDal.GetProducts(productIds);

                    decimal orderTotallMoney = 0;

                    if (products != null && products.Count > 0)
                    {
                        detail.ProductList = new List<Product>();

                        foreach (var subDetail in details)
                        {
                            var productDetail = products.FirstOrDefault(v => v.ProductId == subDetail.ProductId);
                            if (productDetail != null)
                            {
                                var product = new Product();

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
