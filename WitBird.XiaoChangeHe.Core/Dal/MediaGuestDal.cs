using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal.Entity;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    public class MediaGuestDal
    {
        public List<GuestEntity> GetGuests()
        {
            var list = new List<GuestEntity>();

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
SELECT Id, RealName
FROM [dbo].[MediaGuest]";

                var reader = SqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    var entity = new GuestEntity();

                    entity.Id = Convert.ToInt32(reader["Id"]);
                    entity.RealName = reader["RealName"].ToString();

                    list.Add(entity);
                }
            }

            return list;
        }
    }
}
