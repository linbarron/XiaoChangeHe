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
    public class ProductConfigureModel : DbHelper
    {
        #region SelProductConfigureById


        private class SelProductConfigureParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "ProductId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }
        public List<ProductConfigure> SelProductConfigureById(string ProductId)
        {
            List<ProductConfigure> list = null;
            try
            {
                IParameterMapper ipmapper = new SelProductConfigureParameterMapper();
                DataAccessor<ProductConfigure> tableAccessor;
                string strSql = @"select p.ProductId, p.LoveCount,p.Count from ProductConfigure p
                            where p.ProductId=@ProductId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ProductConfigure>.MapAllProperties()
                    .Map(t => t.ProductId).ToColumn("ProductId")
                     .Map(t => t.LoveCount).ToColumn("LoveCount")
                    .Map(t => t.Count).ToColumn("Count")
                    .Build());
                list = tableAccessor.Execute(new string[] { ProductId }).ToList();
                return list;
            }
            catch (Exception ex )
            {
                return null;
            }
        }
        #endregion

        #region SaveProductConfigure
        public int SaveProductConfigure(string type, ProductConfigure info)
        {
            if (string.IsNullOrEmpty(type))
            {
                return 0;
            }

            if (info == null)
            {
                return 0;
            }
            string sql = "";

            DbCommand cmd = null;
            if (type.Equals("Insert"))
            {
                sql = "INSERT INTO ProductConfigure (ProductId,LoveCount,Count)"
                       + " VALUES(@ProductId,@LoveCount,@Count)";

            }
            if (type.Equals("Update"))
            {
                sql = "UPDATE ProductConfigure SET LoveCount=@LoveCount,Count=@Count WHERE ProductId=@ProductId";

            }
            try
            {
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ProductId", DbType.Guid, info.ProductId);
                db.AddInParameter(cmd, "LoveCount", DbType.Int32, info.LoveCount);
                db.AddInParameter(cmd, "Count", DbType.Int32, info.Count);
                return ExecSql(cmd);
            }
            catch (Exception)
            {
            }
            return 0;
        }

        #endregion

        /////<summary>
        /////更新ProductConfigure
        /////</summary>
        /////<param name="Model"></param>
        ///// <returns></returns>
        //public int Update_ProductConfigure(ProductConfigure Model)
        //{
        //    string sqlstr = "@";
        //    string strSql = "";
        //    if (Model.ProductId == null || Model.ProductId == new Guid("00000000-0000-0000-0000-000000000000"))
        //    {
        //        Model.ProductId = Guid.NewGuid();
        //        strSql = string.Format("insert into ProductConfigure (ProductId,LoveCount,Count) values ({0}ProductId,{0}LoveCount,{0}Count)", sqlstr);
        //    }
        //    else
        //    {
        //        strSql = string.Format("UPDATE ProductConfigure set LoveCount={0}LoveCount,Count={0}Count where ProductId={0}ProductId", sqlstr);

        //    }
        //    try
        //    {
        //        DbCommand cmd = db.GetSqlStringCommand(strSql);
        //        db.AddInParameter(cmd, "ProductId", DbType.Guid, Model.ProductId);
        //        db.AddInParameter(cmd, "LoveCount", DbType.Int32, Model.LoveCount);
        //        db.AddInParameter(cmd, "Count", DbType.Int32, Model.Count);
        //        int iRow = db.ExecuteNonQuery(cmd);
        //        return iRow;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}


    }

}