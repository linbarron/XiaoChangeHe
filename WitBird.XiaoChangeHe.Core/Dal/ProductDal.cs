using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal.Entity;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    public class ProductDal
    {
        public List<Product> GetProductsById(List<Guid> productIds)
        {
            List<Product> list = new List<Product>();

            if (productIds != null && productIds.Count > 0)
            {
                using (var SqlConn = ConnectionProvider.GetConnection())
                {
                    var SqlCmd = new SqlCommand();
                    SqlCmd.Connection = SqlConn;
                    SqlCmd.CommandText = @"
select product.Id as ProductId, ProductName
from product
inner join @productIds prod on product.Id = prod.Id";

                    DataTable table = new DataTable();
                    table.Columns.Add("Id", typeof(Guid));
                    foreach (var productId in productIds)
                    {
                        var row = table.NewRow();
                        row["Id"] = productId;
                        table.Rows.Add(row);
                    }
                    SqlParameter parameter = new SqlParameter(@"productIds", SqlDbType.Structured);
                    parameter.TypeName = "TVP_GUID";
                    parameter.Value = table;
                    SqlCmd.Parameters.Add(parameter);

                    var reader = SqlCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var product = new Product();

                        product.ProductId = Guid.Parse(reader["ProductId"].ToString());
                        product.ProductName = reader["ProductName"].ToString();

                        list.Add(product);
                    }
                }
            }

            return list;
        }


    }
}
