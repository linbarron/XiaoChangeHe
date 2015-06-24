using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal.Entity;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    public class OrderDal
    {
        public List<OrderDetails> GetOrderDetails(Guid orderId)
        {
            List<OrderDetails> details = new List<OrderDetails>();


            return details;
        }

        public List<Product> GetProducts(List<Guid> productIds)
        {
            List<Product> products = new List<Product>();

            return products;
        }
    }
}
