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
    public class ProductModel : DbHelper
    {

        #region getProductByProductTypeId 根据产品类型获取产品类型


        private class getProductByProductTypeIdParameterMapper : IParameterMapper
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
                ps1.ParameterName = SqlPara + "ProductTypeId";
                ps1.DbType = DbType.String;
                ps1.Value = parameterValues[1];
                command.Parameters.Add(ps1);
            }
            #endregion

        }



        public List<ProductNew> getProductByProductTypeId(string RestaurantId, string ProductTypeId)
        {
            List<ProductNew> list = null;
            try
            {
                IParameterMapper ipmapper = new getProductByProductTypeIdParameterMapper();
                DataAccessor<ProductNew> tableAccessor;
                string strSql = @" select
                        b.Id,b.ProductName
                      ,b.ProductType
                      ,b.Code
                      ,b.PinYin
                      ,b.Unit
                      ,b.MinUnit
                      ,b.MinCount
                      ,b.Price
                      ,b.BarCode
                      ,b.Status
                      ,b.RestaurantId
                      ,b.Type
                      ,b.MaterialId
                      ,b.IsDiscount 
                      ,b.MemberPrice,cl.CodeTypeListName
                       ,pf.LoveCount,pf.Count,pf.Hot,pf.Description ,pf.Popular from 
                        Product b
                        left join ProductConfigure pf on pf.ProductId=b.Id, S_CodeList cl
                        where 1=1 and b.Status=1 and  b.IsPause=0 and cl.CodeTypeListValue=b.Unit and cl.CodeType='ProductUnit'
                        and not exists(select 1 from productsisbook pb where b.id=pb.Productid and pb.isbook=0 )
                        and b.RestaurantId=@RestaurantId and  b.ProductType=@ProductTypeId order by b.OrderId asc";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ProductNew>.
                    MapAllProperties().Build());
                list = tableAccessor.Execute(new string[] { RestaurantId, ProductTypeId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region getRandomProduct 随机获取6条商品信息


        private class getRandomProductParameterMapper : IParameterMapper
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

        public List<RandomProduct> getRandomProduct(string RestaurantId, string peopleCount)
        {
            List<RandomProduct> list = null;
            try
            {
                IParameterMapper ipmapper = new getRandomProductParameterMapper();
                DataAccessor<RandomProduct> tableAccessor;
                string strSql = @"select top " + peopleCount + "  p.ProductName  , p.id , p.Price,ap.Hot, ap.Popular,ap.ADescription from  product p , Restaurant r, AutoMenusProduct ap" +
                      
                      "  where  p.RestaurantId=r.Id   and p.Id=ap.ProductId and r.id=@RestaurantId and p.Status=1 order by newid() order by p.Code asc";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<RandomProduct>.
                    MapAllProperties().Build());
                list = tableAccessor.Execute(new string[] { RestaurantId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        #endregion
        }





         private class  getAutoRandomProductsParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "CompanyId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
                 DbParameter ps1 = command.CreateParameter();
                ps1.ParameterName = SqlPara + "peopleCount";
                ps1.DbType = DbType.String;
                ps1.Value = parameterValues[1];
                command.Parameters.Add(ps1);

            }
            #endregion

        }
           public List<AutoProducts> getAutoRandomProducts(string CompanyId, string peopleCount)
        {
            List<AutoProducts> list = null;
            try
            {
                IParameterMapper ipmapper = new getAutoRandomProductsParameterMapper();
                DataAccessor<AutoProducts> tableAccessor;
                string strSql = @"exec Intelligent_Product @People=@peopleCount,@CompanyId=@CompanyId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<AutoProducts>.MapAllProperties()
                     .Map(t => t.ProductId).ToColumn("ProductId")
                     .Map(t => t.ProductName).ToColumn("ProductName")
                    .Map(t => t.Price).ToColumn("Price")
                    .Map(t => t.ProductCount).ToColumn("ProductCount")
                    .Map(t => t.Count).ToColumn("Count")
                    .Map(t => t.Description).ToColumn("Description")
                    .Map(t => t.Hot).ToColumn("Hot")
                    .Map(t => t.LoveCount).ToColumn("LoveCount")
                    .Map(t => t.Popular).ToColumn("Popular")
                        
                    .Build());
             
                list = tableAccessor.Execute(new string[] { CompanyId, peopleCount}).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
       
        }


           private class getMemProductsParameterMapper : IParameterMapper
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
           public List<MemProduct> getMemProducts(string CompanyId)
        {
            List<MemProduct> list = null;
            try
            {
                IParameterMapper ipmapper = new getMemProductsParameterMapper();
                DataAccessor<MemProduct> tableAccessor;
                string strSql = @"select p.ProductName ,p.Price,p.MemberPrice ,p.id ,cl.CodeTypeListName
                 from ProductMenu p ,S_CodeList cl where p.Price>p.MemberPrice and p.Status='1'
                    and cl.CodeTypeListValue=p.Unit and cl.CodeType='ProductUnit' and p.CompanyId =@CompanyId order by p.Price";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<MemProduct>.
                    MapAllProperties().Build());
                list = tableAccessor.Execute(new string[] { CompanyId }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

           public List<ProductMenu> getImageProductMenu(string id)
           {
               List<ProductMenu> list = null;
               try
               {
                   IParameterMapper ipmapper = new getMemProductsParameterMapper();
                   DataAccessor<ProductMenu> tableAccessor;
                   string strSql = @"select b.ThumbImage
      from ProductMenu b where 1=1 and b.Status=1 and b.Id=@CompanyId";
                   tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ProductMenu>.MapAllProperties()

                      .Map(t => t.ThumbImage).ToColumn("ThumbImage")

                     .Build());
                   list = tableAccessor.Execute(new string[] { id }).ToList();
                   return list;
               }
               catch (Exception ex)
               {
                   return null;
               }
           }


    }
}