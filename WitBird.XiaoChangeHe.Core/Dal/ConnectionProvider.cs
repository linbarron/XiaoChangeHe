using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal
{
    internal static class ConnectionProvider
    {
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            var SqlConn = new SqlConnection(ConnString);

            SqlConn.Open();

            return SqlConn;
        }
    }
}
