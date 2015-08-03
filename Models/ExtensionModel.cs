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
    public class ExtensionModel : DbHelper
    {
        #region getSubscribeInfo 獲取訂閱信息


        private class getSubscribeInfoParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                //DbParameter ps0 = command.CreateParameter();
                //ps0.ParameterName = SqlPara + "CompanyId";
                //ps0.DbType = DbType.String;
                //ps0.Value = parameterValues[0];
                //command.Parameters.Add(ps0);

                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "EId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }



        public List<Extension> getSubscribeInfo( string EId)
        {
            List<Extension> list = null;
            try
            {
                IParameterMapper ipmapper = new getSubscribeInfoParameterMapper();
                DataAccessor<Extension> tableAccessor;
                string strSql = @" select e.CompanyId,e.Datetime,e.Econtent,e.EditMan,e.EId,e.Etype,e.OrderNo,e.Title ,p.Name
from extension  e , Company p where  e.CompanyId=p.id   and  e.EId=@EId ";

                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<Extension>.
                    MapAllProperties().Build());
                list = tableAccessor.Execute(new string[] {  EId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return null;
            }
        }
        #endregion





        public List<ExtensionImg> getExtensionImg(string Eid)
        {
            List<ExtensionImg> list = null;
            try
            {
                IParameterMapper ipmapper = new getSubscribeInfoParameterMapper();
                DataAccessor<ExtensionImg> tableAccessor;
                string strSql = @" select e.EId,e.Photo from extension  e  where     e.EId=@EId ";

                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ExtensionImg>.
                    MapAllProperties().Map(t => t.Photo).ToColumn("Photo").Build());
                list = tableAccessor.Execute(new string[] {  Eid }).ToList();
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

