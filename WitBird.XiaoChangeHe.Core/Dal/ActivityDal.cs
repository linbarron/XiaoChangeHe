using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal.Entity;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    public class ActivityDal
    {
        public ActivityDetail GetActivityById(int activitytId)
        {
            ActivityDetail detail = null;

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
SELECT
    Adress,
    StartTime,
    EndTime,
    Title,
    ContentText,
    ImageUrl,
    Link
CreatedTime, LastUpdatedTime
FROM dbo.Activity
where Id=@ActivitytId";

                SqlCmd.Parameters.AddWithValue(@"ActivitytId", activitytId);

                var reader = SqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    detail = new ActivityDetail();

                    detail.Address = reader["Adress"] == DBNull.Value ? null : reader["Adress"].ToString();
                    detail.ContentText = reader["ContentText"] == DBNull.Value ? null : reader["ContentText"].ToString();
                    detail.EndTime = reader["EndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndTime"]);
                    detail.ImageUrl = reader["ImageUrl"] == DBNull.Value ? null : reader["ImageUrl"].ToString();
                    detail.Link = reader["ImageUrl"] == DBNull.Value ? null : reader["ImageUrl"].ToString();
                    detail.StartTime = reader["EndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndTime"]);
                    detail.Title = reader["ContentText"] == DBNull.Value ? null : reader["ContentText"].ToString();
                }
            }

            return detail;
        }
    }
}
