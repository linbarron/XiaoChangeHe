using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal.Entity;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    public class StatDal
    {
        public List<GuestDish> GetGuestDishes()
        {
            List<GuestDish> list = new List<GuestDish>();

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
SELECT distinct guest.Id as GuestId, prod.Id AS ProductId
FROM dbo.MediaGuest AS guest INNER JOIN
dbo.Orders AS ord ON guest.GuestUid = ord.MemberCardNo INNER JOIN
dbo.OrderDetails AS detail ON ord.Id = detail.OrderId INNER JOIN
dbo.Product AS prod ON detail.ProductId = prod.Id
";

                var reader = SqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    var entity = new GuestDish();

                    entity.GuestId = Convert.ToInt32(reader["GuestId"]);
                    entity.ProductId = Guid.Parse(reader["ProductId"].ToString());

                    list.Add(entity);
                }
            }

            return list;
        }
    }
}
