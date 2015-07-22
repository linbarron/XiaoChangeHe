using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

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
            catch (Exception ex) 
            {
                throw ex;
            }
            return 0;
        }

        public object ExecuteScalar(DbCommand cmd)
        {
            object result = null;
            try
            {
                result = db.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }

    public static class ExtensionMethod
    {
        public static T TryGetValue<T>(this IDataReader reader, string parameterName)
        {
            T returnValue = default(T);

            try
            {
                returnValue = (T)(reader[parameterName]);
            }
            catch(Exception innerEx)
            {
                Logger.Log(innerEx);
                Exception ex = new Exception("参数转换错误，参数名：" + parameterName, innerEx);
                throw ex;
            }

            return returnValue;
        }

        public static T TryGetValue<T>(this IDataReader reader, int index)
        {
            T returnValue = default(T);

            try
            {
                returnValue = (T)(reader[index]);
            }
            catch(Exception innerEx)
            {
                Logger.Log(innerEx);
                Exception ex = new Exception("参数转换错误，Index：" + index, innerEx);
                throw ex;
            }

            return returnValue;
        }
    }
}