using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Models
{
    public class SelTableInfoModel : DbHelper
    {

        #region SelTableInfo


        private class SelTableInfoParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "RstId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }
        public List<SelTableCount> SelTableInfo(string RstId)
        {
            List<SelTableCount> list = null;
            try
            {
                IParameterMapper ipmapper = new SelTableInfoParameterMapper();
                DataAccessor<SelTableCount> tableAccessor;
                string strSql = @"select s.RstId,
(s.TableCount*s.MaxTime*(cast(s.Rnoon as int)+cast(s.Reven as int))-(select SUM(isnull(o.TableCount,0))
 from Orders o 
left join OrderStatus os on os.OrderId = o.Id
 where 
(os.OrderStatus='Paid' or os.OrderStatus='Confirmed')
and o.RstId=s.RstId)) as total from ReceiveOrder s 
 where s.RstId=@RstId";
                tableAccessor = db.CreateSqlStringAccessor(strSql,ipmapper, MapBuilder<SelTableCount>.MapAllProperties()
                     .Map(t => t.RstId).ToColumn("RstId")
                     .Map(t => t.total).ToColumn("total")
                     //.Map(t => t.MaxTime).ToColumn("MaxTime")
                     //.Map(t => t.Reven).ToColumn("Reven")
                     //.Map(t => t.Rnoon).ToColumn("Rnoon")
                     //.Map(t => t.Status).ToColumn("Status")
                     //.Map(t => t.TableCount).ToColumn("TableCount")
                    .Build());
                list = tableAccessor.Execute(new string[] { RstId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return null;
            }
        }
        #endregion


        public List<SelTableInfo> getTableInfo(string RstId)
        {
            List<SelTableInfo> list = null;
            try
            {
                IParameterMapper ipmapper = new SelTableInfoParameterMapper();
                DataAccessor<SelTableInfo> tableAccessor;
                string strSql = @"exec TableConfing @RstId=@RstId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<SelTableInfo>.MapAllProperties()
                     .Map(t => t.RstName).ToColumn("RstName")
                     .Map(t => t.Times).ToColumn("Times")
                    .Map(t => t.Counts).ToColumn("Counts")
                    .Map(t => t.DiningDate).ToColumn("DiningDate")
                    //.Map(t => t.Rnoon).ToColumn("Rnoon")
                    //.Map(t => t.Status).ToColumn("Status")
                    //.Map(t => t.TableCount).ToColumn("TableCount")
                    .Build());
                list = tableAccessor.Execute(new string[] { RstId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return null;
            }
        }
    }
}