using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    public class UserDal
    {
        public string GetUid(Guid companyId, string sourceAccountId)
        {
            string uid = null;

            using (var SqlConn = ConnectionProvider.GetConnection())
            {
                var SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlConn;
                SqlCmd.CommandText = @"
SELECT [Uid]
FROM [dbo].[CrmMember]
WHERE CompanyId = @CompanyId AND SourceAccountId = @SourceAccountId";

                SqlCmd.Parameters.AddWithValue(@"CompanyId", companyId);
                SqlCmd.Parameters.AddWithValue(@"SourceAccountId", sourceAccountId);

                var reader = SqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    uid = reader["Uid"].ToString();
                }
            }

            return uid;
        }
    }
}
