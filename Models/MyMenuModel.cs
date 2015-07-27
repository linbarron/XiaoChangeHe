using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangHe.Models.Info;
using WitBird.XiaoChangHe;

public class MyMenuModel : DbHelper
{
    private class getMyMenuListDataParameterMapper : IParameterMapper
    {
        #region IParameterMapper 成员

        public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
        {
            DbParameter ps0 = command.CreateParameter();
            ps0.ParameterName = SqlPara + "MemberCardNo";
            ps0.DbType = DbType.String;
            ps0.Value = parameterValues[0];
            command.Parameters.Add(ps0);
            DbParameter ps1 = command.CreateParameter();
            ps1.ParameterName = SqlPara + "OrderId";
            ps1.DbType = DbType.Guid;
            ps1.Value = parameterValues[1];
            command.Parameters.Add(ps1);
        }
        #endregion

    }
    public List<MyMenu> getMyMenuListData(string MemberCardNo, string OrderId, string Type = null)
    {
        List<MyMenu> list = null;
        try
        {
            IParameterMapper ipmapper = new getMyMenuListDataParameterMapper();
            DataAccessor<MyMenu> tableAccessor;
            string strSql = @"select od.UnitPrice,od.ProductCount,o.status,p.MemberPrice,
p.ProductName,p.Id , o.DiningDate ,od.UseState, os.OrderStatus,
(select sum(os.ProductCount) from OrderDetails os where os.OrderId=o.Id) totalcount
from Orders o left join OrderStatus os on os.OrderId = o.Id,OrderDetails od,Product p,Restaurant r  
where
 o.Id=od.OrderId and od.ProductId=p.Id  and r.RstType='02' and r.Id=o.RstId and
o.MemberCardNo=@MemberCardNo and o.id=@OrderId and os.OrderStatus='New' and dateAdd(hh,5,o.DiningDate)>=getdate() order by p.Code asc";
            if (!string.IsNullOrEmpty(Type) && Type == "FastFood")
            {
                strSql = @"select od.UnitPrice,od.ProductCount,o.status,p.MemberPrice,
p.ProductName,p.Id , o.DiningDate ,od.UseState, os.OrderStatus,
(select sum(os.ProductCount) from OrderDetails os where os.OrderId=o.Id) totalcount
from Orders o left join OrderStatus os on os.OrderId = o.Id,OrderDetails od,Product p ,Restaurant r
where
 o.Id=od.OrderId and od.ProductId=p.Id and o.MemberCardNo=@MemberCardNo and o.id=@OrderId 
and os.OrderStatus='New'  and r.RstType='01' and r.Id=o.RstId order by p.Code asc";
            }

            tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<MyMenu>.MapAllProperties()
                 .Map(t => t.ProductName).ToColumn("ProductName")
                 .Map(t => t.UnitPrice).ToColumn("UnitPrice")
                 .Map(t => t.ProductCount).ToColumn("ProductCount")
                 .Map(t => t.ProductId).ToColumn("Id")
                 .Map(t => t.DiningDate).ToColumn("DiningDate")
                 .Map(t => t.status).ToColumn("OrderStatus")
                 .Map(t => t.totalcount).ToColumn("totalcount")
                 .Map(t => t.MemberPrice).ToColumn("MemberPrice")
                  .Map(t => t.UseState).ToColumn("UseState")
                .Build());
            
            list = tableAccessor.Execute(new object[] { MemberCardNo, Guid.Parse(OrderId) }).ToList();
            return list;

        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            return null;
        }
    }

    private class getMyOrderListDataParameterMapper : IParameterMapper
    {
        #region IParameterMapper 成员

        public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
        {
            DbParameter ps0 = command.CreateParameter();
            ps0.ParameterName = SqlPara + "MemberCardNo";
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
    public List<MyOrder> getMyOrderListData(string MemberCardNo, string Type = null)
    {
        List<MyOrder> list = null;
        try
        {
            IParameterMapper ipmapper = new getMyOrderListDataParameterMapper();
            DataAccessor<MyOrder> tableAccessor;
            string strSql = null;
            if (!string.IsNullOrEmpty(Type) && Type == "FastFood")
            {
               // strSql = @"select r.Name,o.PersonCount,o.DiningDate,o.Id ,o.status,r.RstType from Restaurant r, Orders o 
                 //     where r.Id=o.RstId and o.MemberCardNo=@MemberCardNo and r.RstType=01 ";
                 //and o.status=0  

                strSql = @"select r.Name,o.PersonCount,o.DiningDate,o.Id ,o.status, os.OrderStatus, r.RstType ,od.ProductId,p.ProductName" +
                " from Restaurant r, Orders  o left join OrderDetails od on o.Id=od.OrderId left join OrderStatus os on os.OrderId = o.Id,Product p"+
            " where r.Id=o.RstId and o.MemberCardNo=@MemberCardNo  and od.productid=p.id" +
            "  and r.RstType=01 ";
            }
            else
            {
                strSql = @"select r.Name,o.PersonCount,o.DiningDate,o.Id ,o.status, os.OrderStatus, r.RstType ,od.ProductId,p.ProductName" +
                 " from Restaurant r, Orders  o left join OrderDetails od on o.Id=od.OrderId left join OrderStatus os on os.OrderId = o.Id,Product p" +
             " where r.Id=o.RstId and o.MemberCardNo=@MemberCardNo  and od.productid=p.id" +
             "  and r.RstType=02 ";
              //  strSql = @"select r.Name,o.PersonCount,o.DiningDate,o.Id ,o.status,r.RstType from Restaurant r, Orders o 
              // where r.Id=o.RstId and o.MemberCardNo=@MemberCardNo and o.status=1 and r.RstType=02 and  dateAdd(hh,5,o.DiningDate)>=getdate() ";
            }
            tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<MyOrder>.MapAllProperties()
                 .Map(t => t.DiningDate).ToColumn("DiningDate")
                 .Map(t => t.OrderId).ToColumn("Id")
                 .Map(t => t.PersonCount).ToColumn("PersonCount")
                 .Map(t => t.name).ToColumn("name")
                 .Map(t => t.status).ToColumn("OrderStatus")
                  .Map(t => t.RstType).ToColumn("RstType")
                   .Map(t => t.ProductId).ToColumn("ProductId")
                    .Map(t => t.ProductName).ToColumn("ProductName")
                .Build());
            list = tableAccessor.Execute(new string[] { MemberCardNo }).ToList();
            return list;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private class getPresentProductListDataParameterMapper : IParameterMapper
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

    public List<PresentProduct> getPresentProductListData(string RstId)
    {
        List<PresentProduct> list = null;
        try
        {
            IParameterMapper ipmapper = new getPresentProductListDataParameterMapper();
            DataAccessor<PresentProduct> tableAccessor;
            string strSql  = @" 
        select pp.ProductId, pp.ProductName,pp.Unit,pp.MinUnit,pp.RstId ,c.CodeTypeListName ,pp.ProductCount 
        from PresentProduct  pp,s_codelist c 
        where c.codetype='ProductUnit' and pp.unit=c.codetypelistvalue  and pp.Status=1 and pp.RstId=@RstId ";
        tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<PresentProduct>.MapAllProperties()
                 .Map(t => t.ProductId).ToColumn("ProductId")
                 .Map(t => t.MinUnit).ToColumn("MinUnit")
                 .Map(t => t.ProductName).ToColumn("ProductName")
                 .Map(t => t.RstId).ToColumn("RstId")
                 .Map(t => t.CodeTypeListName).ToColumn("CodeTypeListName")
                 .Map(t => t.Unit).ToColumn("Unit")
                 .Map(t => t.ProductCount).ToColumn("ProductCount")
                .Build());
        list = tableAccessor.Execute(new string[] { RstId }).ToList();
            return list;

        }
        catch (Exception ex)
        {
            return null;
        }
    }


    private class getMyOrderDetailDataParameterMapper : IParameterMapper
    {
        #region IParameterMapper 成员

        public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
        {
            DbParameter ps1 = command.CreateParameter();
            ps1.ParameterName = SqlPara + "OrderId";
            ps1.DbType = DbType.String;
            ps1.Value = parameterValues[0];
            command.Parameters.Add(ps1);
        }
        #endregion

    }
    public List<MyOrderDetail> getMyOrderDetailListData(string MemberCardNo, string OrderId, string Type = null)
    {
        List<MyOrderDetail> list = null;
        try
        {
            IParameterMapper ipmapper = new getMyOrderDetailDataParameterMapper();
            DataAccessor<MyOrderDetail> tableAccessor;
            //            string strSql = @"
            //select o.DiningDate,o.PersonCount,o.id,p.id as proId,o.status,
            //o.ContactName,o.ContactPhone,od.ProductCount,od.UnitPrice,
            //p.ProductName,r.Name,r.Address,r.ContactPhone as RstPhone,o.Remark,
            //(select SUM(s.UnitPrice)from OrderDetails s where s.OrderId=o.Id) 
            // as total from Orders o ,OrderDetails od , Restaurant r ,Product  p
            ////where o.Id=od.OrderId and o.RstId=r.Id and od.ProductId=p.Id and o.MemberCardNo=@MemberCardNo and o.id=@OrderId
            //and dateAdd(hh,5,o.DiningDate)>=getdate()and o.status=1  order by o.id ";

            string strSql = null;

            if (!string.IsNullOrEmpty(Type) && Type == "FastFood")
            {
                strSql = @"
select a.DiningDate,a.PersonCount,a.id,p.id as proId, os.OrderStatus as status, p.MemberPrice,
a.ContactName,a.ContactPhone,od.ProductCount,od.UnitPrice,od.UseState,
p.ProductName,a.Name,a.Address,a.RstPhone,a.Remark,a.total,a.RstType 
 from 
(select o.DiningDate,o.PersonCount,o.id,o.status, r.RstType, o.ContactName,o.ContactPhone,r.Name,r.Address,r.ContactPhone as RstPhone,o.Remark,
(select SUM(isnull(s.UnitPrice,0)*isnull(s.ProductCount,0))  from OrderDetails s where s.OrderId=o.Id) as total  
from Orders o,Restaurant r where o.RstId=r.Id
and o.id=@OrderId) a 
left join OrderDetails od on a.Id=od.OrderId
left join OrderStatus os on os.OrderId = a.Id 
left join Product  p on od.ProductId=p.Id
";//o.status=0

            }
            else
            {
                strSql = @"select a.DiningDate,a.PersonCount,a.id,p.id as proId, os.OrderStatus as status, p.MemberPrice,
a.ContactName,a.ContactPhone,od.ProductCount,od.UnitPrice,od.UseState,
p.ProductName,a.Name,a.Address,a.RstPhone,a.Remark,a.total,a.RstType
 from 
(select o.DiningDate,o.PersonCount,o.id,o.status, r.RstType,
o.ContactName,o.ContactPhone,r.Name,r.Address,r.ContactPhone as RstPhone,o.Remark,
(select SUM(isnull(s.UnitPrice,0)*isnull(s.ProductCount,0))  from OrderDetails s where s.OrderId=o.Id) as total  
from Orders o,Restaurant r where o.RstId=r.Id
and o.id=@OrderId) a 
left join OrderDetails od on a.Id=od.OrderId
left join OrderStatus os on os.OrderId = a.Id
left join Product  p on od.ProductId=p.Id
";
            }
            tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<MyOrderDetail>.MapAllProperties()
                 .Map(t => t.DiningDate).ToColumn("DiningDate")
                 .Map(t => t.address).ToColumn("address")
                 .Map(t => t.RstType).ToColumn("RstType")
                 .Map(t => t.PersonCount).ToColumn("PersonCount")
                 .Map(t => t.RstName).ToColumn("name")
                 .Map(t => t.ContactName).ToColumn("ContactName")
                 .Map(t => t.ContactPhone).ToColumn("ContactPhone")
                 .Map(t => t.ProductCount).ToColumn("ProductCount")
                 .Map(t => t.RstPhone).ToColumn("RstPhone")
                 .Map(t => t.Remark).ToColumn("Remark")
                 .Map(t => t.UnitPrice).ToColumn("UnitPrice")
                 .Map(t => t.ProductName).ToColumn("ProductName")
                 .Map(t => t.OrderId).ToColumn("id")
                 .Map(t => t.total).ToColumn("total")
                 .Map(t => t.proId).ToColumn("proId")
                 .Map(t => t.status).ToColumn("status")
                 .Map(t => t.UseState).ToColumn("UseState")
                .Build());
            list = tableAccessor.Execute(new string[] { OrderId }).ToList();
            return list;

        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            return null;
        }
    }

}
