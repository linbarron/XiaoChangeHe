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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">1.正在进行中的。2，已经结束的</param>
        /// <returns></returns>
        public List<Activity> GetActivities(int state)
        {
            List<Activity> list = new List<Activity>();

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
select Id,Title, ImageUrl
from Activity
where State = @State
order by LastUpdatedTime desc";

                SqlCmd.Parameters.AddWithValue(@"State", state);

                var reader = SqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    var activity = new Activity();

                    activity.Id = Convert.ToInt32(reader["Id"]);

                    if (reader["Title"] != DBNull.Value)
                    {
                        activity.Title = reader["Title"].ToString();
                    }
                    if (reader["ImageUrl"] != DBNull.Value)
                    {
                        activity.ImageUrl = reader["ImageUrl"].ToString();
                    }
                    if (reader["Description"] != DBNull.Value)
                    {
                        activity.Description = reader["Description"].ToString();
                    }

                    list.Add(activity);
                }
            }

            return list;
        }
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
