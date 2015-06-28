using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    internal class RestaurantDal
    {
        public string GetRestaurantName(Guid restaurantId)
        {
            string restaurantName = string.Empty;

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
SELECT [Name]
FROM [dbo].[Restaurant]
where Id=@RestaurantId";

                SqlCmd.Parameters.AddWithValue(@"RestaurantId", restaurantId);

                var reader = SqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    restaurantName = reader["Name"].ToString();
                }
            }

            return restaurantName;
        }
    }
}
