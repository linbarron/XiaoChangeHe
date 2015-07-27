using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangeHe.Core;

namespace WitBird.XiaoChangHe.Models
{
    public class OrderModel : DbHelper
    {
        #region getRestaurantListInfoData


        private class getProductListInfoDataParameterMapper : IParameterMapper
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



        public List<Product> getProductListInfoData(string id)
        {
            List<Product> list = null;
            try
            {
                IParameterMapper ipmapper = new getProductListInfoDataParameterMapper();
                DataAccessor<Product> tableAccessor;
                string strSql = @"select case when a.TypeName is null then a.TypeName else a.TypeName end as ProductTypeName1,
a.TypeName as ProductTypeName2,
case when a.TypeName is null then a.OrderNo else a.OrderNo end as OrderNo
,b.Id,b.ProductName
      ,b.ProductType
      ,b.Code
      ,b.PinYin
      ,b.Unit
      ,b.MinUnit
      ,b.MinCount
      ,b.Price
      ,null as OriginalImage
      ,null as ThumbImage
      ,b.BarCode
      ,b.Status
      ,b.RestaurantId
      ,b.Type
      ,b.MaterialId
      ,b.IsDiscount 
       ,a.IsServiceType,pf.OrderCount,pf.FavCount from ProductType a 
inner join Product b on a.RestaurantId=b.RestaurantId and a.TypeId=b.ProductType
left join ProductFav pf on pf.ProductId=b.Id
where 1=1 and b.Status=1 and a.IsServiceType=0 and b.RestaurantId=@RestaurantId
order by OrderNo,a.OrderNo";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<Product>.
                    MapAllProperties()
                    /*.Map(t => t.ProductTypeName1).ToColumn("ProductTypeName1")
                    .Map(t => t.ProductTypeName2).ToColumn("ProductTypeName2")
                    .Map(t => t.OrderNo).ToColumn("OrderNo")
                    .Map(t => t.Id).ToColumn("Id")
                    //.Map(t => t.TypeId).ToColumn("TypeId")
                    .Map(t => t.ProductName).ToColumn("ProductName")
                    .Map(t => t.ProductType).ToColumn("ProductType")
                    .Map(t => t.Code).ToColumn("Code")
                    .Map(t => t.PinYin).ToColumn("PinYin")
                    .Map(t => t.Unit).ToColumn("Unit")
                    .Map(t => t.MinUnit).ToColumn("MinUnit")
                    .Map(t => t.MinCount).ToColumn("MinCount")
                    .Map(t => t.Price).ToColumn("Price")
                    .Map(t => t.OriginalImage).ToColumn("OriginalImage")
                    .Map(t => t.ThumbImage).ToColumn("ThumbImage")
                    .Map(t => t.BarCode).ToColumn("BarCode")
                    .Map(t => t.Status).ToColumn("Status")
                    .Map(t => t.RestaurantId).ToColumn("RestaurantId")
                    .Map(t => t.Type).ToColumn("Type")
                    .Map(t => t.MaterialId).ToColumn("MaterialId")
                    .Map(t => t.IsDiscount).ToColumn("IsDiscount")
                    .Map(t => t.IsServiceType).ToColumn("IsServiceType")
                    */
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

        #region getImageProductListInfoData


        private class getImageProductListInfoDataParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "Id";
                ps0.DbType = DbType.Guid;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }
        public List<Product> getOrginalImageProductListInfoData(string id)
        {
            List<Product> list = null;
            try
            {
                IParameterMapper ipmapper = new getImageProductListInfoDataParameterMapper();
                DataAccessor<Product> tableAccessor;
                string strSql = @"select b.OriginalImage
      from Product b where 1=1 and b.Status=1 and b.Id=@Id";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<Product>.MapAllProperties()
                     .DoNotMap(t => t.ProductTypeName1)
                     .DoNotMap(t => t.ProductTypeName2)
                     .DoNotMap(t => t.OrderNo)
                     .DoNotMap(t => t.Id)
                    //.DoNotMap(t => t.TypeId)
                     .DoNotMap(t => t.ProductName)
                     .DoNotMap(t => t.ProductType)
                     .DoNotMap(t => t.Code)
                     .DoNotMap(t => t.PinYin)
                     .DoNotMap(t => t.Unit)
                     .DoNotMap(t => t.MinUnit)
                     .DoNotMap(t => t.MinCount)
                     .DoNotMap(t => t.Price)
                     .Map(t => t.OriginalImage).ToColumn("OriginalImage")
                     .DoNotMap(t => t.ThumbImage)
                     .DoNotMap(t => t.BarCode)
                     .DoNotMap(t => t.Status)
                     .DoNotMap(t => t.RestaurantId)
                     .DoNotMap(t => t.Type)
                     .DoNotMap(t => t.MaterialId)
                     .DoNotMap(t => t.IsDiscount)
                     .DoNotMap(t => t.IsServiceType)
                     .DoNotMap(t => t.OrderCount)
                     .DoNotMap(t => t.FavCount)
                    .Build());
                list = tableAccessor.Execute(new object[] { Guid.Parse(id) }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Logger.Log("ID: " + id + "\r\n" + ex);
                return null;
            }
        }

        public List<Product> getThumbImageProductListInfoData(string id)
        {
            List<Product> list = null;
            try
            {
                IParameterMapper ipmapper = new getImageProductListInfoDataParameterMapper();
                DataAccessor<Product> tableAccessor;
                string strSql = @"select b.ThumbImage
      from Product b where 1=1 and b.Status=1 and b.Id=@Id";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<Product>.MapAllProperties()
                     .DoNotMap(t => t.ProductTypeName1)
                     .DoNotMap(t => t.ProductTypeName2)
                     .DoNotMap(t => t.OrderNo)
                     .DoNotMap(t => t.Id)
                    //.DoNotMap(t => t.TypeId)
                     .DoNotMap(t => t.ProductName)
                     .DoNotMap(t => t.ProductType)
                     .DoNotMap(t => t.Code)
                     .DoNotMap(t => t.PinYin)
                     .DoNotMap(t => t.Unit)
                     .DoNotMap(t => t.MinUnit)
                     .DoNotMap(t => t.MinCount)
                     .DoNotMap(t => t.Price)
                     .Map(t => t.ThumbImage).ToColumn("ThumbImage")
                     .DoNotMap(t => t.OriginalImage)
                     .DoNotMap(t => t.BarCode)
                     .DoNotMap(t => t.Status)
                     .DoNotMap(t => t.RestaurantId)
                     .DoNotMap(t => t.Type)
                     .DoNotMap(t => t.MaterialId)
                     .DoNotMap(t => t.IsDiscount)
                     .DoNotMap(t => t.IsServiceType)
                     .DoNotMap(t => t.OrderCount)
                     .DoNotMap(t => t.FavCount)
                    .Build());
                list = tableAccessor.Execute(new object[] { Guid.Parse(id) }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Logger.Log("ID: " + id + "\r\n" + ex);
                return null;
            }
        }

        public List<ReceiveOrder1> getDefauleImage(string id)
        {
            List<ReceiveOrder1> list = null;
            try
            {
                IParameterMapper ipmapper = new getImageProductListInfoDataParameterMapper();
                DataAccessor<ReceiveOrder1> tableAccessor;
                string strSql = @"select ro.DefaultImg from Product p , 
        ReceiveOrder ro where p.RestaurantId=ro.RstId  and p.Id=@Id";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<ReceiveOrder1>.MapAllProperties()

                     .Map(t => t.DefaultImg).ToColumn("DefaultImg")

                    .Build());
                list = tableAccessor.Execute(new object[] { Guid.Parse(id) }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Logger.Log("ID: " + id + "\r\n" + ex);
                return null;
            }
        }
        #endregion

        private class selOrderIdParameterMapper : IParameterMapper
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
        #region selOrderId
        public Order SelectPaidOrder(string uid)
        {
            Order paidOrder = null;

            try
            {
                string sql = @"
select top 1 o.*, os.OrderStatus, cm.Sex
from Orders o
left join OrderStatus os on o.Id = os.OrderId
left join CrmMember cm on cm.Uid = o.MemberCardNo
where o.MemberCardNo = @Uid
and os.OrderStatus = 'Paid';";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "Uid", DbType.String, uid);

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        paidOrder = new Order()
                        {
                            ContactName = reader.TryGetValue<String>("ContactName"),
                            ContactPhone = reader.TryGetValue<String>("ContactPhone"),
                            CreateDate = reader.TryGetValue<DateTime>("CreateDate"),
                            DiningDate = reader.TryGetValue<DateTime>("DiningDate"),
                            Id = reader.TryGetValue<Guid>("Id"),
                            MemberCardNo = reader.TryGetValue<String>("MemberCardNo"),
                            OperatorId = reader.TryGetValue<Guid>("OperatorId"),
                            OperatorName = reader.TryGetValue<String>("OperatorName"),
                            PersonCount = reader.TryGetValue<Int32?>("PersonCount"),
                            PrepayPrice = reader.TryGetValue<Decimal>("PrepayPrice"),
                            Remark = reader.TryGetValue<String>("Remark"),
                            ReserveType = reader.TryGetValue<String>("ReserveType"),
                            Sex = reader.TryGetValue<Boolean>("Sex"),
                            Status = reader.TryGetValue<String>("OrderStatus"),
                            TableCount = reader.TryGetValue<Int32?>("TableCount")
                        };
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Log(exception);
                throw;
            }

            return paidOrder;
        }
        #endregion
        #region  SelUnFinValidOrder 查询未完成且未过期的订单
//        public List<Order> SelUnFinValidOrder(string id)
//        {
//            List<Order> list = null;
//            try
//            {
//                IParameterMapper ipmapper = new selOrderIdParameterMapper();
//                DataAccessor<Order> tableAccessor;
//                string strSql = @"
//select 
//b.ContactName, b.ContactPhone,b.CreateDate,
//b.DiningDate,b.MemberCardNo,b.Id,b.OperatorId,b.OperatorName,b.PersonCount,b.PrepayPrice,b.Remark,b.ReserveType,
//b.Status,b.TableCount, c.sex ,b.RstId 
//from Orders b, crmmember c 
//where 
//b.MemberCardNo=@OrderId 
//and b.Status=0 
//and  dateAdd(hh,5,b.DiningDate)>=getdate() 
//and b.MemberCardNo=c.Uid";

//                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<Order>.MapAllProperties()
//                     .Map(t => t.Id).ToColumn("Id")
//                     .Map(t => t.Status).ToColumn("Status")
//                     .Map(t => t.ContactName).ToColumn("ContactName")
//                     .Map(t => t.ContactPhone).ToColumn("ContactPhone")
//                     .Map(t => t.CreateDate).ToColumn("CreateDate")
//                     .Map(t => t.DiningDate).ToColumn("DiningDate")
//                     .Map(t => t.MemberCardNo).ToColumn("MemberCardNo")
//                     .Map(t => t.OperatorId).ToColumn("OperatorId")
//                     .Map(t => t.OperatorName).ToColumn("OperatorName")
//                     .Map(t => t.PersonCount).ToColumn("PersonCount")
//                     .Map(t => t.PrepayPrice).ToColumn("PrepayPrice")
//                     .Map(t => t.Remark).ToColumn("Remark")
//                     .Map(t => t.ReserveType).ToColumn("ReserveType")
//                     .Map(t => t.TableCount).ToColumn("TableCount")
//                     .Map(t => t.Sex).ToColumn("Sex")
//                     .Map(t => t.RstId).ToColumn("RstId")
//                                   .Build());
//                list = tableAccessor.Execute(new string[] { id }).ToList();
//                return list;

//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

        public Order SelectUnFinishedOrder(string uid)
        {
            Order unFinishedOrder = null;

            try
            {
                string sql = @"
select top 1 o.*, os.OrderStatus, cm.Sex
from Orders o
left join OrderStatus os on o.Id = os.OrderId
left join CrmMember cm on cm.Uid = o.MemberCardNo
where o.MemberCardNo = @Uid
and os.OrderStatus = 'New';";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "Uid", DbType.String, uid);

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        unFinishedOrder = new Order()
                        {
                            ContactName = reader.TryGetValue<String>("ContactName"),
                            ContactPhone = reader.TryGetValue<String>("ContactPhone"),
                            CreateDate = reader.TryGetValue<DateTime>("CreateDate"),
                            DiningDate = reader.TryGetValue<DateTime>("DiningDate"),
                            Id = reader.TryGetValue<Guid>("Id"),
                            MemberCardNo = reader.TryGetValue<String>("MemberCardNo"),
                            OperatorId = reader.TryGetValue<Guid>("OperatorId"),
                            OperatorName = reader.TryGetValue<String>("OperatorName"),
                            PersonCount = reader.TryGetValue<Int32?>("PersonCount"),
                            PrepayPrice = reader.TryGetValue<Decimal>("PrepayPrice"),
                            Remark = reader.TryGetValue<String>("Remark"),
                            ReserveType = reader.TryGetValue<String>("ReserveType"),
                            Sex = reader.TryGetValue<Boolean>("Sex"),
                            Status = reader.TryGetValue<String>("OrderStatus"),
                            TableCount = reader.TryGetValue<Int32?>("TableCount")
                        };
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Log(exception);
                throw;
            }

            return unFinishedOrder;
        }

        #endregion

        #region selOrder  查询为快餐店产生的订单

        public Order SelectUnFinishedFastFoodOrder(string uid)
        {
            Order unFinishedFastFoodOrder = null;

            try
            {
                string sql = @"
select top 1 o.*, os.OrderStatus, r.RstType
from Orders o
left join Restaurant r on r.Id = o.RstId
left join OrderStatus os on o.Id = os.OrderId
where o.MemberCardNo = @Uid
and os.OrderStatus = 'New'
and r.RstType='01';";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "Uid", DbType.String, uid);

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        unFinishedFastFoodOrder = new Order()
                        {
                            ContactName = reader.TryGetValue<String>("ContactName"),
                            ContactPhone = reader.TryGetValue<String>("ContactPhone"),
                            CreateDate = reader.TryGetValue<DateTime>("CreateDate"),
                            DiningDate = reader.TryGetValue<DateTime>("DiningDate"),
                            Id = reader.TryGetValue<Guid>("Id"),
                            MemberCardNo = reader.TryGetValue<String>("MemberCardNo"),
                            OperatorId = reader.TryGetValue<Guid>("OperatorId"),
                            OperatorName = reader.TryGetValue<String>("OperatorName"),
                            PersonCount = reader.TryGetValue<Int32?>("PersonCount"),
                            PrepayPrice = reader.TryGetValue<Decimal>("PrepayPrice"),
                            Remark = reader.TryGetValue<String>("Remark"),
                            ReserveType = reader.TryGetValue<String>("ReserveType"),
                            //Sex = reader.TryGetValue<Boolean>("Sex"),
                            Status = reader.TryGetValue<String>("OrderStatus"),
                            TableCount = reader.TryGetValue<Int32?>("TableCount"),
                            RstType = reader.TryGetValue<String>("RstType")
                        };
                    }
                }
            }
            catch(Exception exception)
            {
                Logger.Log(exception);
                throw;
            }

            return unFinishedFastFoodOrder;
        }
        #endregion

        #region selOrderByOrderId

        public List<Order> selOrderByOrderId(Guid orderId)
        {
            List<Order> list = null;
            try
            {
                IParameterMapper ipmapper = new selOrderIdParameterMapper();
                DataAccessor<Order> tableAccessor;
                string strSql = @"select b.ContactName, b.ContactPhone,b.CreateDate,
b.DiningDate,b.MemberCardNo,b.Id,b.OperatorId,b.OperatorName,b.PersonCount,b.PrepayPrice,b.Remark,b.ReserveType,
b.Status, os.OrderStatus, b.TableCount, c.sex ,b.RstId from Orders b left join OrderStatus os on os.OrderId = b.Id, crmmember c where b.id=@OrderId and b.Status=0 
and  dateAdd(hh,5,b.DiningDate)>=getdate() and b.MemberCardNo=c.Uid";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<Order>.MapAllProperties()
                     .Map(t => t.Id).ToColumn("Id")
                     .Map(t => t.Status).ToColumn("OrderStatus")
                     .Map(t => t.ContactName).ToColumn("ContactName")
                     .Map(t => t.ContactPhone).ToColumn("ContactPhone")
                     .Map(t => t.CreateDate).ToColumn("CreateDate")
                     .Map(t => t.DiningDate).ToColumn("DiningDate")
                     .Map(t => t.MemberCardNo).ToColumn("MemberCardNo")
                     .Map(t => t.OperatorId).ToColumn("OperatorId")
                     .Map(t => t.OperatorName).ToColumn("OperatorName")
                     .Map(t => t.PersonCount).ToColumn("PersonCount")
                     .Map(t => t.PrepayPrice).ToColumn("PrepayPrice")
                     .Map(t => t.Remark).ToColumn("Remark")
                     .Map(t => t.ReserveType).ToColumn("ReserveType")
                     .Map(t => t.TableCount).ToColumn("TableCount")
                     .Map(t => t.Sex).ToColumn("Sex")
                     .Map(t => t.RstId).ToColumn("RstId")
                                   .Build());
                list = tableAccessor.Execute(new string[] { orderId.ToString() }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region SaveOrders
        public int SaveOrders(string type, Order info)
        {
            if (string.IsNullOrEmpty(type))
            {
                return 0;
            }

            if (info == null)
            {
                return 0;
            }
            string sql;
            DbCommand cmd = null;
            if (type.Equals("Insert"))
            {
                sql = "begin tran ;INSERT INTO Orders(Id,ContactName,ContactPhone,DiningDate,"
                       + "PersonCount,TableCount,MemberCardNo,ReserveType,PrepayPrice,Remark,Status,OperatorId,OperatorName,CreateDate,RstId)"
                       + " VALUES (@Id,@ContactName,@ContactPhone,@DiningDate,@PersonCount,@TableCount,@MemberCardNo,"
                       + "@ReserveType,@PrepayPrice,@Remark,@Status,@OperatorId,@OperatorName,@CreateDate,@RstId)"
                       + ";insert into OrderStatus(OrderId, OrderStatus, LastUpdateTime) values(@OrderId, @OrderStatus, @LastUpdateTime); commit tran";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "Id", DbType.Guid, info.Id);
                db.AddInParameter(cmd, "ContactName", DbType.String, info.ContactName);
                db.AddInParameter(cmd, "ContactPhone", DbType.String, info.ContactPhone);
                db.AddInParameter(cmd, "DiningDate", DbType.DateTime, info.DiningDate);

                db.AddInParameter(cmd, "PersonCount", DbType.Int32, info.PersonCount);
                db.AddInParameter(cmd, "TableCount", DbType.Int32, info.TableCount);

                db.AddInParameter(cmd, "MemberCardNo", DbType.String, info.MemberCardNo);
                db.AddInParameter(cmd, "ReserveType", DbType.String, info.ReserveType);

                db.AddInParameter(cmd, "PrepayPrice", DbType.Decimal, info.PrepayPrice);
                db.AddInParameter(cmd, "Remark", DbType.String, info.Remark);
                db.AddInParameter(cmd, "Status", DbType.Boolean, false);

                db.AddInParameter(cmd, "OperatorId", DbType.Guid, info.OperatorId);
                db.AddInParameter(cmd, "OperatorName", DbType.String, info.OperatorName);
                db.AddInParameter(cmd, "RstId", DbType.Guid, info.RstId);

                db.AddInParameter(cmd, "OrderId", DbType.Guid, info.Id);
                db.AddInParameter(cmd, "OrderStatus", DbType.String, OrderStatus.New);
                db.AddInParameter(cmd, "LastUpdateTime", DbType.DateTime, info.CreateDate);

            }
            else if (type.Equals("Update"))
            {
                sql = "UPDATE Orders SET ContactName=@ContactName,ContactPhone=@ContactPhone,"
                + "PersonCount=@PersonCount,TableCount=@TableCount"
                + ",Remark=@Remark,Status=@Status"
                + ",CreateDate=@CreateDate WHERE Id=@Id";

                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "Id", DbType.Guid, info.Id);
                db.AddInParameter(cmd, "ContactName", DbType.String, info.ContactName);
                db.AddInParameter(cmd, "ContactPhone", DbType.String, info.ContactPhone);

                db.AddInParameter(cmd, "PersonCount", DbType.Int32, info.PersonCount);
                db.AddInParameter(cmd, "TableCount", DbType.Int32, info.TableCount);
                db.AddInParameter(cmd, "Remark", DbType.String, info.Remark);

                db.AddInParameter(cmd, "Status", DbType.Boolean, info.Status);

            }
            else if (type.Equals("UpdateFastFood"))
            {

                sql = "UPDATE Orders SET Remark=@Remark,CreateDate=@CreateDate WHERE Id=@Id";

                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "Id", DbType.Guid, info.Id);

                db.AddInParameter(cmd, "Remark", DbType.String, info.Remark);



            }
            try
            {

                db.AddInParameter(cmd, "CreateDate", DbType.DateTime, info.CreateDate);

                return ExecSql(cmd);
            }
            catch (Exception exception)
            {
                Logger.Log(exception);
            }
            return 0;
        }

        #endregion

        #region EmptyOrder
        public int EmptyOrder(string orderId)
        {
            string sql = "delete from Orders WHERE  Id=@OrderId";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "OrderId", DbType.String, orderId);


            return ExecSql(cmd);

        }

        public int EmptyOrderStatus(string orderId)
        {
            string sql = "delete from OrderStatus WHERE  OrderId=@OrderId";
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "OrderId", DbType.Guid, Guid.Parse(orderId));


            return ExecSql(cmd);
        }
        #endregion EmptyOrder


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(Guid orderId, string orderStatus)
        {
            bool result = false;

            try
            {
                //Logger.Log(LoggingLevel.WxPay, "订单ID：" + orderId + ", 更新状态：" + orderStatus);
                DbCommand cmd = null;
                string sql = @"UPDATE [CrmRstCloud].[dbo].[OrderStatus] SET OrderStatus = @orderStatus WHERE OrderId = @orderId;";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "OrderStatus", DbType.String, orderStatus);
                db.AddInParameter(cmd, "orderId", DbType.Guid, orderId);

                result = ExecSql(cmd) > 0;

                //Logger.Log(LoggingLevel.WxPay, "订单状态更新成功");
            }
            catch (Exception ex)
            {
                //Logger.Log(LoggingLevel.WxPay, "订单状态更新失败");
                Logger.Log(LoggingLevel.WxPay, ex);
            }

            return result;
        }
    }


}