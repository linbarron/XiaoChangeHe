using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class AutoMenusModel : DbHelper
    {
        #region getAutoProduct


        private class getAutoProductParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "RestaurantId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
                DbParameter ps1 = command.CreateParameter();
                ps1.ParameterName = SqlPara + "PeopleCount";
                ps1.DbType = DbType.String;
                ps1.Value = parameterValues[1];
                command.Parameters.Add(ps1);
            }
            #endregion

        }
        public List<AutoMenuAndProduct> getAutoProduct(string RestaurantId, string PeopleCount)
        {
            List<AutoMenuAndProduct> list = null;
            try
            {
                if (string.IsNullOrEmpty(RestaurantId) && string.IsNullOrEmpty(PeopleCount))
                {
                    return null;
                }
                IParameterMapper ipmapper = new getAutoProductParameterMapper();
                DataAccessor<AutoMenuAndProduct> tableAccessor;
                string strSql = @"select a.ID,a.Datatime,a.Description,a.Name,a.PeopleCount,a.Status,p.ProductName,p.Price
 ,ap.AID,ap.AutoMenusId,ap.ADescription,ap.Hot,ap.OrderId,ap.Popular,ap.ProductId
 ,ap.RestaurantId,ap.AStatus,ap.type
 from AutoMenu a ,AutoMenusProduct ap,Product p 
 where a.ID=ap.AutoMenusId  AND  ap.ProductId=p.Id and a.PeopleCount=@PeopleCount and ap.RestaurantId= @RestaurantId
 and a.Status=1 and ap.AStatus=1 and p.IsPause=1 order by ap.OrderId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<AutoMenuAndProduct>.MapAllProperties()
                     .Map(t => t.ADescription).ToColumn("ADescription")
                     .Map(t => t.AID).ToColumn("AID")
                     .Map(t => t.AStatus).ToColumn("AStatus")
                     .Map(t => t.Datatime).ToColumn("Datatime")
                     .Map(t => t.Description).ToColumn("Description")
                     .Map(t => t.Hot).ToColumn("Hot")
                     .Map(t => t.ID).ToColumn("ID")
                     .Map(t => t.Name).ToColumn("Name")
                     .Map(t => t.OrderId).ToColumn("OrderId")
                     .Map(t => t.Popular).ToColumn("Popular")
                     .Map(t => t.ProductId).ToColumn("ProductId")
                     .Map(t => t.RestaurantId).ToColumn("RestaurantId")
                     .Map(t => t.Status).ToColumn("Status")
                     .Map(t => t.type).ToColumn("type")
                       .Map(t => t.ProductName).ToColumn("ProductName")
                        .Map(t => t.Price).ToColumn("Price")
                    .Build());
                list = tableAccessor.Execute(new string[] { RestaurantId,PeopleCount }).ToList();
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