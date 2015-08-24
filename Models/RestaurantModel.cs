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
    public class RestaurantModel:DbHelper
    {
        #region getRestaurantListInfoData


        private class getRestaurantListInfoDataParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "SourceAccountId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }

//        public List<RestaurantAbstract> getRestaurentState(string companyid)
//        {
//            try
//            {
//                //IParameterMapper ipmapper = new getRestaurantListInfoDataParameterMapper();
//                DataAccessor<RestaurantAbstract> tableAccessor;
//                //                string strSql = @"select a.Id
//                string strSql = @"select rst.Id,rst.Name,rst.ContactPhone,rst.Address,rst.BusinessStartDate,
//rst.BusinessEndtDate,rst.RstType,ro.MapUrl,ro.VirtualUrl, null as Photo ,
// isnull(ro.MaxTime,0) as MaxTime ,case when (ro.Reven=1 or ro.Rnoon=1)  then 1 else 0 end as isAcceptOrder
// from Restaurant rst left join ReceiveOrder ro on rst.Id=ro.RstId where rst.CompanyId='" + companyid + "'";

//                tableAccessor = db.CreateSqlStringAccessor(strSql, MapBuilder<RestaurantAbstract>.MapAllProperties()
//                        .Build());
//                return tableAccessor.Execute().ToList();

//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
        //        }
        #region getRestaurentStateIParameterMapper 成员
        private class getRestaurentStateParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "CompanyId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }
        public List<RestaurantAbstract> getRestaurentState(string CompanyId)
        {
            try
            {
                IParameterMapper ipmapper = new getRestaurentStateParameterMapper();
                DataAccessor<RestaurantAbstract> tableAccessor;
                string strSql = @"
 select rst.Id,rst.Name,rst.ContactPhone,rst.Address,rst.BusinessStartDate,
rst.BusinessEndtDate,rst.RstType,ro.MapUrl,ro.VirtualUrl, null as Photo ,p.Name,
 isnull(ro.MaxTime,0) as MaxTime ,case when (ro.Reven=1 or ro.Rnoon=1)  then 1 else 0 end as isAcceptOrder
 from Restaurant rst left join ReceiveOrder ro on rst.Id=ro.RstId ,Province p
 where rst.CompanyId=@CompanyId and rst.usestate='01'  
 and rst.CityId=p.Id and p.IsUse='1'";

                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper,MapBuilder<RestaurantAbstract>.MapAllProperties()
                        .Build());
                return tableAccessor.Execute(new string[] { CompanyId }).ToList(); ;

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return null;
            }
        }

        #endregion
//        public List<Restaurant> getRestaurantListInfoData(string companyid)
//        {
//            List<Restaurant> list = null;
//            try
//            {
//                DataAccessor<Restaurant> tableAccessor;
//                string strSql = @"select a.Id
//      ,a.Name
//      ,a.CompanyId
//      ,a.Address
//      ,a.ContactPhone
//      ,a.Description
//      ,a.BankId
//      ,a.BankAccount
//      ,a.OpenDate
//      ,a.BusinessStartDate
//      ,a.BusinessEndtDate
//      ,a.RegSN
//      ,a.ReserveModel
//      ,a.UseState
//      ,a.PublicKey
//      ,a.PrivateKey from Restaurant a where a.CompanyId='"+companyid+"'";
//                tableAccessor = db.CreateSqlStringAccessor(strSql, MapBuilder<Restaurant>.MapAllProperties()
//                     .Map(t => t.Id).ToColumn("Id")
//                     .Map(t => t.Name).ToColumn("Name")
//                     .Map(t => t.CompanyId).ToColumn("CompanyId")
//                     .Map(t => t.Address).ToColumn("Address")
//                     .Map(t => t.ContactPhone).ToColumn("ContactPhone")
//                     .Map(t => t.Description).ToColumn("Description")
//                     .Map(t => t.BankId).ToColumn("BankId")
//                     .Map(t => t.BankAccount).ToColumn("BankAccount")
//                     .Map(t => t.OpenDate).ToColumn("OpenDate")
//                     .Map(t => t.BusinessStartDate).ToColumn("BusinessStartDate")
//                     .Map(t => t.BusinessEndtDate).ToColumn("BusinessEndtDate")
//                     .Map(t => t.RegSN).ToColumn("RegSN")
//                     .Map(t => t.ReserveModel).ToColumn("ReserveModel")
//                     .Map(t => t.UseState).ToColumn("UseState")
//                     .Map(t => t.PublicKey).ToColumn("PublicKey")
//                     .Map(t => t.PrivateKey).ToColumn("PrivateKey")
                
//                    .Build());
//                list = tableAccessor.Execute().ToList();
//                return list;

//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }
        #endregion
        #region getImageRst  

        public List<RestaurantAbstract> getImageRst(string id)
        {
            List<RestaurantAbstract> list = null;
            try
            {
                IParameterMapper ipmapper = new getRestaurantListInfoDataParameterMapper();
                DataAccessor<RestaurantAbstract> tableAccessor;
                string strSql = @"select ro.photo
 from  ReceiveOrder   ro
 where ro.RstId=@SourceAccountId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<RestaurantAbstract>.MapAllProperties()
                     .DoNotMap(t => t.Id)
                     .DoNotMap(t => t.isAcceptOrder)
                     .DoNotMap(t => t.MapUrl)
                     .DoNotMap(t => t.MaxTime)
                    //.DoNotMap(t => t.TypeId)
                     .DoNotMap(t => t.Name)
                     .DoNotMap(t => t.RstType)
                     .DoNotMap(t => t.VirtualUrl)
                     .DoNotMap(t => t.Address)
                     .DoNotMap(t => t.BusinessEndtDate)
                     .DoNotMap(t => t.BusinessStartDate)
                     .DoNotMap(t => t.ContactPhone)
                     .DoNotMap(t=>t.name)
                  
                     .Map(t => t.Photo).ToColumn("Photo")
                    
                    .Build());
                list = tableAccessor.Execute(new string[] { id }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return null;
            }
        }
        #endregion


        #region WitBird新建

        private class GetRestaurentByIdParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "restaurantId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }

        public NewRestaurantAbstract GetRestaurentById(string restaurantId)
        {
            try
            {
                IParameterMapper ipmapper = new GetRestaurentByIdParameterMapper();
                DataAccessor<NewRestaurantAbstract> tableAccessor;
                string strSql = @"select rst.Id,rst.Name,rst.ContactPhone,rst.Address,rst.BusinessStartDate,
rst.BusinessEndtDate,rst.RstType,rst.[Description],ro.MapUrl,ro.VirtualUrl, null as Photo ,p.Name,
 isnull(ro.MaxTime,0) as MaxTime ,case when (ro.Reven=1 or ro.Rnoon=1)  then 1 else 0 end as isAcceptOrder
 from Restaurant rst left join ReceiveOrder ro on rst.Id=ro.RstId ,Province p
 where rst.Id=@restaurantId;";

                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<NewRestaurantAbstract>.MapAllProperties()
                        .Build());
                return tableAccessor.Execute(new string[] { restaurantId }).ToList().FirstOrDefault();

            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}