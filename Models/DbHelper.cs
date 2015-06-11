using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WitBird.XiaoChangHe.Models
{
    public abstract class DbHelper
    {
        public Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("CrmRstV1");
        public static string SqlPara = "@";
        public int ExecSql(DbCommand cmd)
        {
            try
            {

                if (db.ExecuteNonQuery(cmd) > 0)
                {
                    return 1;
                }
            }
            catch (Exception)
            {
            }
            return 0;
        }
    }
}