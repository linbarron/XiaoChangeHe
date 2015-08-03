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
    public class ProductMenuTypeModel: DbHelper
    {

        #region ProductMenuType


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
        public List<ProductMenuType> getProductMenuType(string RstId)
        {
            List<ProductMenuType> list = null;
            try
            {
                IParameterMapper ipmapper = new SelTableInfoParameterMapper();
                DataAccessor<ProductMenuType> tableAccessor;
                string strSql = @"select  p.CompanyId,p.IsServiceType,p.OrderNo,p.ParentType,p.PrintId,p.TypeId,p.TypeName 
from ProductMenuType p where IsServiceType='0' order by p.OrderNo ";
              //  tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ProductMenuType>.MapAllProperties()
                tableAccessor = db.CreateSqlStringAccessor(strSql, MapBuilder<ProductMenuType>.MapAllProperties()

                     .Map(t => t.CompanyId).ToColumn("CompanyId")
                     .Map(t => t.IsServiceType).ToColumn("IsServiceType")
                    .Map(t => t.OrderNo).ToColumn("OrderNo")
                    .Map(t => t.ParentType).ToColumn("ParentType")
                    .Map(t => t.PrintId).ToColumn("PrintId")
                    .Map(t => t.TypeId).ToColumn("TypeId")
                    .Map(t => t.TypeName).ToColumn("TypeName")
                    .Build());
               // list = tableAccessor.Execute(new string[] { RstId }).ToList();
                list = tableAccessor.Execute().ToList();
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