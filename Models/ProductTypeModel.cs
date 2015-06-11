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
    public class ProductTypeModel : DbHelper
    {
        #region getProductType 获取产品类型


        private class getProductTypeParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "RestaurantId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }



        public List<ProductType> getProductType(string RestaurantId)
        {
            List<ProductType> list = null;
            try
            {
                IParameterMapper ipmapper = new getProductTypeParameterMapper();
                DataAccessor<ProductType> tableAccessor;
                string strSql = @" select
                                    pt.TypeId,
                                    pt.TypeName,
                                    pt.IsServiceType,
                                    pt.OrderNo,
                                    pt.ParentType,
                                    pt.PrintId,
                                    pt.RestaurantId
                                     from ProductType pt 
                                     where pt.IsServiceType=0  and pt.RestaurantId=@RestaurantId
                                    and pt.ParentType!='00000000-0000-0000-0000-000000000000'
                                    and exists(select 1 from Product p where pt.TypeId=p.ProductType and p.Status=1)
                                    and not exists(select 1 from producttypeisbook pb where pt.TypeId=pb.ProductTypeid and pb.isbook=0 )
                                    order by pt.OrderNo  ";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ProductType>.
                    MapAllProperties().Build());
                list = tableAccessor.Execute(new string[] { RestaurantId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public List<ProductType> getOneProductType(string RestaurantId)
        {
            List<ProductType> list = null;
            try
            {
                IParameterMapper ipmapper = new getProductTypeParameterMapper();
                DataAccessor<ProductType> tableAccessor;
                string strSql = @" select
                                    pt.TypeId,
                                    pt.TypeName,
                                    pt.IsServiceType,
                                    pt.OrderNo,
                                    pt.ParentType,
                                    pt.PrintId,
                                    pt.RestaurantId
                                     from ProductType pt 
                                     where pt.IsServiceType=0  and pt.RestaurantId=@RestaurantId
                                    and pt.ParentType='00000000-0000-0000-0000-000000000000'

                                    and not exists(select 1 from producttypeisbook pb where pt.TypeId=pb.ProductTypeid and pb.isbook=0 )
                                    order by pt.OrderNo  ";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ProductType>.
                    MapAllProperties().Build());
                list = tableAccessor.Execute(new string[] { RestaurantId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        #region getProductType 根据一级菜单ID获取产品类型


        private class getProductTypebyIdParameterMapper : IParameterMapper
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
                ps1.ParameterName = SqlPara + "ParentType";
                ps1.DbType = DbType.String;
                ps1.Value = parameterValues[1];
                command.Parameters.Add(ps1);
            }
            #endregion

        }



        public List<ProductType> getProductTypebyId(string RestaurantId, string ParentType)
        {
            List<ProductType> list = null;
            try
            {
                IParameterMapper ipmapper = new getProductTypebyIdParameterMapper();
                DataAccessor<ProductType> tableAccessor;
                string strSql = @" select
                                    pt.TypeId,
                                    pt.TypeName,
                                    pt.IsServiceType,
                                    pt.OrderNo,
                                    pt.ParentType,
                                    pt.PrintId,
                                    pt.RestaurantId
                                     from ProductType pt 
                                     where pt.IsServiceType=0  and pt.RestaurantId=@RestaurantId
                                     and pt.ParentType=@ParentType
                                    and pt.ParentType!='00000000-0000-0000-0000-000000000000'
                                    and exists(select 1 from Product p where pt.TypeId=p.ProductType and p.Status=1)
                                    and not exists(select 1 from producttypeisbook pb where pt.TypeId=pb.ProductTypeid and pb.isbook=0 )
                                    order by pt.OrderNo  ";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ProductType>.
                    MapAllProperties().Build());
                list = tableAccessor.Execute(new string[] { RestaurantId, ParentType }).ToList();
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