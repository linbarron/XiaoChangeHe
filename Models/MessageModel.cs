using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
namespace WitBird.XiaoChangHe.Models
{
    public class MessageModel : DbHelper
    {
        private class getMessageParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "CompanyId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
                //DbParameter ps1 = command.CreateParameter();
                //ps1.ParameterName = SqlPara + "OrderId";
                //ps1.DbType = DbType.String;
                //ps1.Value = parameterValues[1];
                //command.Parameters.Add(ps1);
            }
            #endregion

        }
        public List<AboutUs> getAboutUsListData(string CompanyId,string name)
        {
            List<AboutUs> list = null;
            try
            {
                IParameterMapper ipmapper = new getMessageParameterMapper();
                DataAccessor<AboutUs> tableAccessor;
                string strSql = @"select a.CompanyId ,a.Contents,a.Datetime,a.ID,a.Title 
        from AboutUs  a where a.CompanyId=@CompanyId and a.ID='" + name + "'";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<AboutUs>.MapAllProperties()
                     .Map(t => t.ID).ToColumn("ID")
                     
                     .Map(t => t.Title).ToColumn("Title")
                     .Map(t => t.Contents).ToColumn("Contents")
                      .Map(t => t.CompanyId).ToColumn("CompanyId")
                    .Map(t => t.DateTime).ToColumn("DateTime")
                    .Build());
                list = tableAccessor.Execute(new string[] { CompanyId }).ToList();
               // list = tableAccessor.Execute().ToList(new string[] { CompanyId }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return null;
            }
        }
        public List<AboutUs> getPromotionListData(string CompanyId)
        {
            List<AboutUs> list = null;
            try
            {
                IParameterMapper ipmapper = new getMessageParameterMapper();
                DataAccessor<AboutUs> tableAccessor;
                string strSql = @"select a.CompanyId ,a.Contents,a.Datetime,a.ID,a.Title 
        from news  a where a.CompanyId=@CompanyId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<AboutUs>.MapAllProperties()
                     .Map(t => t.ID).ToColumn("ID")
                     .Map(t => t.Title).ToColumn("Title")
                     .Map(t => t.Contents).ToColumn("Contents")
                      .Map(t => t.CompanyId).ToColumn("CompanyId")
                    .Map(t => t.DateTime).ToColumn("DateTime")
                    .Build());
                 list = tableAccessor.Execute(new string[] { CompanyId  }).ToList();
               // list = tableAccessor.Execute().ToList(new string[] { CompanyId }).ToList();
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
