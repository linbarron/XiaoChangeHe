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
    public class ReceiveOrderModel : DbHelper
    {
        #region SelNewsInfo


        private class SelReceiveOrder2InfoParameterMapper : IParameterMapper
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
        public List<ReceiveOrder2> SelReceiveOrder2Info(string RstId)
        {
            List<ReceiveOrder2> list = null;
            try
            {
                IParameterMapper ipmapper = new SelReceiveOrder2InfoParameterMapper();
                DataAccessor<ReceiveOrder2> tableAccessor;
                string strSql = @"select r.Explain from  ReceiveOrder r 
                    where r.RstId=@RstId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ReceiveOrder2>.MapAllProperties()
                     .Map(t => t.Explain).ToColumn("Explain")
                    .Build());
                list = tableAccessor.Execute(new string[] { RstId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion



    }
}