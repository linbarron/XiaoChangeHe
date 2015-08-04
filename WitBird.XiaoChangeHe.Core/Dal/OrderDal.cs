using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal.Entity;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    public class OrderDal
    {
        public List<OrderSummary> GetUserOrders(string memberCardNo)
        {
            List<OrderSummary> list = new List<OrderSummary>();

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
select o.Id AS OrderId,o.ContactName,o.ContactPhone,o.DiningDate,o.DiningDate,o.CreateDate,
o.Status,o.RstId AS RestaurantId,o.PersonCount, os.OrderStatus, os.LastUpdateTime, 
-- Caculates order total price
case when pr.PrepayMoney is NULL then
(select SUM(od.ProductCount * od.UnitPrice) from OrderDetails od where od.OrderId=o.Id)
else pr.PrepayMoney end as TotalMoney
from orders o
left join OrderStatus os on os.OrderId = o.Id
left join PrepayRecord pr on pr.SId = convert(nvarchar(50), o.Id)
where 
((pr.AddMoney = pr.PrepayMoney and pr.AddMoney <= 0) or (pr.AddMoney is null and pr.PrepayMoney is null))
and o.MemberCardNo=@MemberCardNo
order by o.CreateDate desc;
";

                SqlCmd.Parameters.AddWithValue(@"MemberCardNo", memberCardNo);

                var reader = SqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["TotalMoney"] == DBNull.Value)
                    { continue; }

                    var summary = new OrderSummary();

                    summary.OrderId = Guid.Parse(reader["OrderId"].ToString());
                    summary.ContactName = reader["ContactName"].ToString();
                    if (reader["ContactPhone"] != DBNull.Value)
                    {
                        summary.ContactPhone = reader["ContactPhone"].ToString();
                    }
                    if (reader["PersonCount"] != DBNull.Value)
                    {
                        summary.PersonCount = Convert.ToInt32(reader["PersonCount"]);
                    }
                    if (reader["OrderStatus"] != DBNull.Value)
                    {
                        summary.Status = Convert.ToString(reader["OrderStatus"]);
                    }
                    if (reader["DiningDate"] != DBNull.Value)
                    {
                        summary.DiningDate = Convert.ToDateTime(reader["DiningDate"]);
                    }
                    if (reader["TotalMoney"] != DBNull.Value)
                    {
                        summary.TotalMoney = Math.Abs(Convert.ToDecimal(reader["TotalMoney"]));
                    }
                    summary.RestaurantId = Guid.Parse(reader["RestaurantId"].ToString());

                    summary.CreateTime = Convert.ToDateTime(reader["CreateDate"]);
                    summary.Backlog = "无";

                    list.Add(summary);
                }
            }

            if (list != null && list.Count > 0)
            {
                RestaurantDal restaurantDal = new RestaurantDal();

                foreach (var item in list)
                {
                    item.RestaurantName = restaurantDal.GetRestaurantName(item.RestaurantId);
                }
            }

            return list;
        }

        public OrderSummary GetOrderSummary(Guid orderId)
        {
            OrderSummary summary = null;

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
select o.Id AS OrderId,o.ContactName,o.ContactPhone,o.DiningDate,o.DiningDate,o.CreateDate,
o.Status,o.RstId AS RestaurantId,o.PersonCount, os.OrderStatus, os.LastUpdateTime, o.Sex, 
-- Caculates order total price
case when pr.PrepayMoney is NULL then
(select SUM(od.ProductCount * od.UnitPrice) from OrderDetails od where od.OrderId=o.Id)
else pr.PrepayMoney end as TotalMoney
from orders o
left join OrderStatus os on os.OrderId = o.Id
left join PrepayRecord pr on pr.SId = convert(nvarchar(50), o.Id)
where ((pr.AddMoney = pr.PrepayMoney and pr.AddMoney <= 0) or (pr.AddMoney is null and pr.PrepayMoney is null))
and o.Id=@OrderId";

                SqlCmd.Parameters.AddWithValue(@"OrderId", orderId);

                using (var reader = SqlCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        summary = new OrderSummary();

                        summary.OrderId = Guid.Parse(reader["OrderId"].ToString());
                        summary.ContactName = reader["ContactName"].ToString();
                        if (reader["sex"] != DBNull.Value)
                        {
                            int sexValue = Convert.ToInt32(reader["sex"]);
                            if (sexValue == 0)
                            {
                                summary.ContactName += "先生";
                            }
                            else
                            {
                                summary.ContactName += "女士";
                            }
                        }
                        if (reader["PersonCount"] != DBNull.Value)
                        {
                            summary.PersonCount = Convert.ToInt32(reader["PersonCount"]);
                        }
                        if (reader["ContactPhone"] != DBNull.Value)
                        {
                            summary.ContactPhone = reader["ContactPhone"].ToString();
                        }
                        if (reader["OrderStatus"] != DBNull.Value)
                        {
                            summary.Status = Convert.ToString(reader["OrderStatus"]);
                        }
                        if (reader["DiningDate"] != DBNull.Value)
                        {
                            summary.DiningDate = Convert.ToDateTime(reader["DiningDate"]);
                        }

                        if (reader["TotalMoney"] != DBNull.Value)
                        {
                            summary.TotalMoney = Math.Abs(Convert.ToDecimal(reader["TotalMoney"]));
                        }
                        else
                        {
                            summary.TotalMoney = GetOrderDetails(summary.OrderId).First().TotalPrice;
                        }
                        summary.CreateTime = Convert.ToDateTime(reader["CreateDate"]);
                        summary.Backlog = "无";
                    }
                }
            }

            return summary;
        }

        public List<OrderDetails> GetOrderDetails(Guid orderId)
        {
            List<OrderDetails> details = new List<OrderDetails>();

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
select o.ProductId, o.ProductCount, o.UnitPrice, o.TotalPrice, 
case when pr.PrepayMoney is NULL then
(select SUM(od.ProductCount * od.UnitPrice) from OrderDetails od where od.OrderId=o.OrderId)
else pr.PrepayMoney end as TotalMoney
from OrderDetails o
left join PrepayRecord pr on pr.SId = convert(nvarchar(50), o.OrderId)
left join OrderBillPay bp on bp.PayId = pr.BillPayId
where (bp.PayState = '0x01' or bp.PayState is null)
and o.OrderId=@OrderId";

                SqlCmd.Parameters.AddWithValue(@"OrderId", orderId);

                var reader = SqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    var detail = new OrderDetails();

                    detail.ProductId = Guid.Parse(reader["ProductId"].ToString());
                    detail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    detail.ProductCount = Convert.ToInt32(reader["ProductCount"]);

                    if (reader["TotalMoney"] != DBNull.Value)
                    {
                        detail.TotalPrice = Math.Abs(Convert.ToDecimal(reader["TotalMoney"]));
                    }
                    else
                    {
                        detail.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);
                    }

                    details.Add(detail);
                }
            }

            return details;
        }

        public List<Product> GetProducts(List<Guid> productIds)
        {
            List<Product> products = new List<Product>();

            if (productIds != null && productIds.Count > 0)
            {
                foreach (var productId in productIds)
                {
                    var product = GetProductById(productId);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        private Product GetProductById(Guid productId)
        {
            Product product = null;

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
select Id as ProductId, ProductName,ThumbImage
from product
where Id=@ProductId";

                SqlCmd.Parameters.AddWithValue(@"ProductId", productId);

                var reader = SqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product();

                    product.ProductId = Guid.Parse(reader["ProductId"].ToString());
                    product.ProductName = reader["ProductName"].ToString();
                    product.Image = reader["ThumbImage"] == DBNull.Value ? null : (byte[])reader["ThumbImage"];
                }
            }

            return product;
        }
    }
}
