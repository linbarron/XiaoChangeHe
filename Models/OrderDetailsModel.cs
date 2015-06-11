using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.XiaoChangHe.Models.Info;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
namespace WitBird.XiaoChangHe.Models
{
    public class OrderDetailsModel : DbHelper
    {

        private class getOrderDetailsListInfoDataParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "OrderId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
                DbParameter ps1 = command.CreateParameter();
                ps1.ParameterName = SqlPara + "ProductId";
                ps1.DbType = DbType.String;
                ps1.Value = parameterValues[1];
                command.Parameters.Add(ps1);
            }
            #endregion
        }

        public List<OrderDetails> getOrderDetailInfoData(string productId, string orderId)
        {
            List<OrderDetails> list = null;
            try
            {
                IParameterMapper ipmapper = new getOrderDetailsListInfoDataParameterMapper();
                DataAccessor<OrderDetails> tableAccessor;
                string strSql = @"select od.CreateDate,
                                    od.DetailsId,
                                    od.Discount ,
                                    od.DiscountPrice,
                                    od.OrderId,
                                    od.ProductCount,
                                    od.Remark,
                                    od.SendState,
                                    od.TotalPrice,
                                    od.UnitPrice,
                                    od.UseState ,
                                    od.ProductId ,
                                     p.ProductName
                                    from OrderDetails od  ,Product p  
                    where od.ProductId=p.Id and  od.ProductId=@ProductId  and od.OrderId=@OrderId";
                 tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper,  MapBuilder<OrderDetails>.MapAllProperties()
             
                     .Map(t => t.CreateDate).ToColumn("CreateDate")
                     .Map(t => t.DetailsId).ToColumn("DetailsId")
                     .Map(t => t.Discount).ToColumn("Discount")
                     .Map(t => t.DiscountPrice).ToColumn("DiscountPrice")
                     .Map(t => t.OrderId).ToColumn("OrderId")
                     .Map(t => t.ProductCount).ToColumn("ProductCount")
                     .Map(t => t.Remark).ToColumn("Remark")
                     .Map(t => t.SendState).ToColumn("SendState")
                     .Map(t => t.TotalPrice).ToColumn("TotalPrice")
                     .Map(t => t.UnitPrice).ToColumn("UnitPrice")
                     .Map(t => t.UseState).ToColumn("UseState")
                     .Map(t => t.ProductId).ToColumn("ProductId")
                     .Map(t => t.ProductName).ToColumn("ProductName")
                    .Build());
                list = tableAccessor.Execute(new string[] {orderId , productId }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private class getOrderDetaiByIdParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "OrderId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
               
            }
            #endregion
        }
        public List<OrderDetails> getOrderDetaiById(string orderId)
        {
            List<OrderDetails> list = null;
            try
            {
                IParameterMapper ipmapper = new getOrderDetaiByIdParameterMapper();
                DataAccessor<OrderDetails> tableAccessor;
                string strSql = @"
select od.CreateDate,
                                    od.DetailsId,
                                    od.Discount ,
                                    od.DiscountPrice,
                                    od.OrderId,
                                    od.ProductCount,
                                    od.Remark,
                                    od.SendState,
                                    od.TotalPrice,
                                    od.UnitPrice,
                                    od.UseState ,
                                    od.ProductId ,
                                     p.ProductName
                                    from OrderDetails od  left join   Product p  
                on od.ProductId=p.Id and od.OrderId=@OrderId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<OrderDetails>.MapAllProperties()

                    .Map(t => t.CreateDate).ToColumn("CreateDate")
                    .Map(t => t.DetailsId).ToColumn("DetailsId")
                    .Map(t => t.Discount).ToColumn("Discount")
                    .Map(t => t.DiscountPrice).ToColumn("DiscountPrice")
                    .Map(t => t.OrderId).ToColumn("OrderId")
                    .Map(t => t.ProductCount).ToColumn("ProductCount")
                    .Map(t => t.Remark).ToColumn("Remark")
                    .Map(t => t.SendState).ToColumn("SendState")
                    .Map(t => t.TotalPrice).ToColumn("TotalPrice")
                    .Map(t => t.UnitPrice).ToColumn("UnitPrice")
                    .Map(t => t.UseState).ToColumn("UseState")
                    .Map(t => t.ProductId).ToColumn("ProductId")
                    .Map(t => t.ProductName).ToColumn("ProductName")
                   .Build());
                list = tableAccessor.Execute(new string[] { orderId }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region SaveOrderDetails
        //public int SaveOrderDetails(string type, OrderDetails info)
        public int SaveOrderDetails(string type, OrderDetails info)
        {
            if (string.IsNullOrEmpty(type))
            {
                return 0;
            }

            if (info == null)
            {
                return 0;
            }
            DbCommand cmd = null;
            string sql;
            if (type.Equals("Insert"))
            {
                sql = "INSERT INTO OrderDetails(DetailsId,OrderId,ProductId,ProductCount,UnitPrice,TotalPrice,"
                    + "UseState,SendState,Remark,Discount,DiscountPrice,CreateDate)VALUES (@DetailsId,@OrderId,@ProductId,"
                    + "@ProductCount,@UnitPrice,@TotalPrice,@UseState,@SendState,@Remark,@Discount,@DiscountPrice,@CreateDate)";

                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "DetailsId", DbType.Guid, info.DetailsId);
                db.AddInParameter(cmd, "OrderId", DbType.Guid, info.OrderId);
                db.AddInParameter(cmd, "ProductId", DbType.Guid, info.ProductId);
                db.AddInParameter(cmd, "ProductCount", DbType.Int32, info.ProductCount);

                db.AddInParameter(cmd, "UnitPrice", DbType.Decimal, info.UnitPrice);
                db.AddInParameter(cmd, "TotalPrice", DbType.Decimal, info.TotalPrice);

                db.AddInParameter(cmd, "UseState", DbType.String, info.UseState);
                db.AddInParameter(cmd, "SendState", DbType.String, info.SendState);

                db.AddInParameter(cmd, "Remark", DbType.String, info.Remark);
                db.AddInParameter(cmd, "Discount", DbType.Decimal, info.Discount);
                db.AddInParameter(cmd, "DiscountPrice", DbType.Decimal, info.DiscountPrice);
                //db.AddInParameter(cmd, "CreateDate", DbType.DateTime, info.CreateDate);
            }
            if (type.Equals("Update"))
            {
                sql = "UPDATE OrderDetails  set ProductCount= @ProductCount,CreateDate = @CreateDate,TotalPrice=@TotalPrice ,UseState=@UseState WHERE ProductId=@ProductId and OrderId=@OrderId";
                cmd = db.GetSqlStringCommand(sql);
                //db.AddInParameter(cmd, "DetailsId", DbType.Guid, info.DetailsId);
                db.AddInParameter(cmd, "OrderId", DbType.Guid, info.OrderId);
                db.AddInParameter(cmd, "ProductId", DbType.Guid, info.ProductId);
                db.AddInParameter(cmd, "ProductCount", DbType.Int32, info.ProductCount);
                //db.AddInParameter(cmd, "CreateDate", DbType.DateTime, info.CreateDate);
                //db.AddInParameter(cmd, "UnitPrice", DbType.Decimal, info.UnitPrice);
                db.AddInParameter(cmd, "TotalPrice", DbType.Decimal, info.TotalPrice);

                db.AddInParameter(cmd, "UseState", DbType.String, info.UseState);
                //db.AddInParameter(cmd, "SendState", DbType.String, info.SendState);

                //db.AddInParameter(cmd, "Remark", DbType.String, info.Remark);
                //db.AddInParameter(cmd, "Discount", DbType.Decimal, info.Discount);
                //db.AddInParameter(cmd, "DiscountPrice", DbType.Decimal, info.DiscountPrice);
            }
            // "INSERT INTO OrderDetails(DetailsId,OrderId,ProductId,ProductCount,UnitPrice,TotalPrice,UseState,SendState,Remark,Discount,DiscountPrice,CreateDate)VALUES (@DetailsId,@OrderId,@ProductId,@ProductCount,@UnitPrice,@TotalPrice,@UseState,@SendState,@Remark,@Discount,@DiscountPrice,@CreateDate)";
            try
            {


                db.AddInParameter(cmd, "CreateDate", DbType.DateTime, info.CreateDate);
                return ExecSql(cmd);
            }
            catch (Exception)
            {
            }
            return 0;
        }

        #endregion 

        #region 删除赠送菜
        public int DelGiftOrderDetails(string UseState, string orderId)
        {
            string sql = "delete from OrderDetails WHERE UseState=@UseState and OrderId=@OrderId";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "OrderId", DbType.String, orderId);
            db.AddInParameter(cmd, "UseState", DbType.String, UseState);

            return ExecSql(cmd);

        }
        #endregion DelOrderDetails


        #region DelOrderDetails
        public int DelOrderDetails(string productId, string orderId) {
            string sql = "delete from OrderDetails WHERE ProductId=@ProductId and OrderId=@OrderId";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "OrderId", DbType.String, orderId);
            db.AddInParameter(cmd, "ProductId", DbType.String, productId);
            
            return ExecSql(cmd);
            
        }
        #endregion DelOrderDetails

        #region DelOrderDetails
        public int EmptyOrderDetails(string MemberCardNo, string orderId)
        { 
            string sql = "delete from OrderDetails WHERE  OrderId=@OrderId";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "OrderId", DbType.String, orderId);
          

            return ExecSql(cmd);

        }
        #endregion DelOrderDetails


      
    }


}