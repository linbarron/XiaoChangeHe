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
    public class NewsModel : DbHelper
    {
        #region SelNewsInfo


        private class SelNewsInfoParameterMapper : IParameterMapper
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
        public List<News> SelNewsInfo(string RstId)
        {
            List<News> list = null;
            try
            {
                IParameterMapper ipmapper = new SelNewsInfoParameterMapper();
                DataAccessor<News> tableAccessor;
                string strSql = @"select n.ID,n.Title,n.CompanyId,n.Contents,n.Datetime  from news n
                    where n.CompanyId=@RstId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<News>.MapAllProperties()
                     .Map(t => t.CompanyId).ToColumn("CompanyId")
                     .Map(t => t.Contents).ToColumn("Contents")
                    .Map(t => t.DateTime).ToColumn("DateTime")
                    .Map(t => t.ID).ToColumn("ID")
                    .Map(t => t.Title).ToColumn("Title")
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

    }

}