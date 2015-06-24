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

            var details = orderDal.GetOrderDetails(orderId);

            if (details != null) 
            {
                detail = new OrderDetail();

                var productIds = details.Select(v => v.ProductId).ToList();

                var products = orderDal.GetProducts(productIds);
            }

            return detail;
        }
    }
}
