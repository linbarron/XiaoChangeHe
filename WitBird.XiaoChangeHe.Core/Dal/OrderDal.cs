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
select Id AS OrderId,ContactName,ContactPhone,DiningDate,DiningDate,CreateDate,Status,RstId AS RestaurantId,PersonCount
from orders
where MemberCardNo=@MemberCardNo
order by CreateDate desc";

                SqlCmd.Parameters.AddWithValue(@"MemberCardNo", memberCardNo);

                var reader = SqlCmd.ExecuteReader();
                while (reader.Read())
                {
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
                    if (reader["Status"] != DBNull.Value)
                    {
                        summary.Status = Convert.ToBoolean(reader["Status"]);
                    }
                    if (reader["DiningDate"] != DBNull.Value)
                    {
                        summary.DiningDate = Convert.ToDateTime(reader["DiningDate"]);
                    }
                    summary.RestaurantId = Guid.Parse(reader["RestaurantId"].ToString());

                    summary.CreateTime = Convert.ToDateTime(reader["CreateDate"]);
                    summary.Backlog = "无";

                    var details = this.GetOrderDetails(summary.OrderId);
                    if (details != null && details.Count > 0)
                    {
                        summary.TotalMoney = details.Sum(v => v.TotalPrice);
                    }

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
select Id as OrderId,ContactName,ContactPhone,DiningDate,CreateDate,Status,sex,PersonCount
from Orders
where Id=@OrderId";

                SqlCmd.Parameters.AddWithValue(@"OrderId", orderId);

                var reader = SqlCmd.ExecuteReader();
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
                        else if (sexValue == 1)
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
                    if (reader["Status"] != DBNull.Value)
                    {
                        summary.Status = Convert.ToBoolean(reader["Status"]);
                    }
                    if (reader["DiningDate"] != DBNull.Value)
                    {
                        summary.DiningDate = Convert.ToDateTime(reader["DiningDate"]);
                    }
                    summary.CreateTime = Convert.ToDateTime(reader["CreateDate"]);
                    summary.Backlog = "无";
                }
            }

            if (summary != null)
            {
                var details = this.GetOrderDetails(summary.OrderId);
                if (details != null && details.Count > 0)
                {
                    summary.TotalMoney = details.Sum(v => v.TotalPrice);
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
select ProductId, ProductCount,UnitPrice,TotalPrice
from OrderDetails
where OrderId=@OrderId";

                SqlCmd.Parameters.AddWithValue(@"OrderId", orderId);

                var reader = SqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    var detail = new OrderDetails();

                    detail.ProductId = Guid.Parse(reader["ProductId"].ToString());
                    detail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    detail.ProductCount = Convert.ToInt32(reader["ProductCount"]);
                    detail.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);

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
                    product.Image = (byte[])reader["ThumbImage"];
                }
            }

            return product;
        }
    }
}
