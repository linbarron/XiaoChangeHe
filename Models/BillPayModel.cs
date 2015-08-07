using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Models
{
    public class OrderBillPayModel : DbHelper
    {
        /// <summary>
        /// 查询支付订单
        /// </summary>
        /// <param name="payId"></param>
        /// <returns></returns>
        public OrderBillPay GetBillPayById(Guid payId)
        {
            OrderBillPay billPay = null;

            try
            {
                string sql = @"select top 1 * from OrderBillPay where PayId = @PayId";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "PayId", DbType.Guid, payId);

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        billPay = new OrderBillPay()
                        {
                            PayId = payId,
                            Cash = reader.TryGetValue<Decimal>("Cash"),
                            Change = reader.TryGetValue<Decimal>("Change"),
                            Coupons = reader.TryGetValue<Decimal>("Coupons"),
                            CouponsNo = reader.TryGetValue<String>("CouponsNo"),
                            CreateDate = reader.TryGetValue<DateTime>("CreateDate"),
                            CreditCard = reader.TryGetValue<Decimal>("CreditCard"),
                            MemberCard = reader.TryGetValue<Decimal>("MemberCard"),
                            MemberCardNo = reader.TryGetValue<String>("MemberCardNo"),
                            PaidIn = reader.TryGetValue<Decimal>("PaidIn"),
                            PayState = reader.TryGetValue<String>("PayState"),
                            Receivable = reader.TryGetValue<Decimal>("Receivable"),
                            Remark = reader.TryGetValue<String>("Remark"),
                            Remove = reader.TryGetValue<Decimal>("Remove"),
                            UserId = reader.TryGetValue<Guid>("UserId"),
                            UserName = reader.TryGetValue<String>("UserName"),
                            RstId = reader.TryGetValue<Guid>("RstId"),
                            Discount = reader.TryGetValue<Decimal>("Discount")
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(LoggingLevel.WxPay, ex);
            }

            return billPay;
        }

        /// <summary>
        /// 增加支付订单记录
        /// </summary>
        /// <param name="billPay"></param>
        /// <returns></returns>
        public bool AddBillPay(OrderBillPay billPay)
        {
            bool result = false;

            try
            {
                DbCommand cmd = null;
                string sql = @"
                            INSERT INTO [CrmRstCloud].[dbo].[OrderBillPay]
                                ([PayId]
                                ,[Receivable]
                                ,[PaidIn]
                                ,[Change]
                                ,[Remove]
                                ,[MemberCardNo]
                                ,[Cash]
                                ,[CreditCard]
                                ,[MemberCard]
                                ,[Coupons]
                                ,[CouponsNo]
                                ,[Remark]
                                ,[PayState]
                                ,[UserId]
                                ,[UserName]
                                ,[CreateDate]
                                ,[Discount]
                                ,[RstId])
                            VALUES
                            (
                                @PayId
                                ,@Receivable
                                ,@PaidIn
                                ,@Change
                                ,@Remove
                                ,@MemberCardNo
                                ,@Cash
                                ,@CreditCard
                                ,@MemberCard
                                ,@Coupons
                                ,@CouponsNo
                                ,@Remark
                                ,@PayState
                                ,@UserId
                                ,@UserName
                                ,@CreateDate
                                ,@Discount
                                ,@RstId
                            );";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "PayId", DbType.Guid, billPay.PayId);
                db.AddInParameter(cmd, "Receivable", DbType.Decimal, billPay.Receivable);
                db.AddInParameter(cmd, "PaidIn", DbType.Decimal, billPay.PaidIn);
                db.AddInParameter(cmd, "Change", DbType.Decimal, billPay.Change);
                db.AddInParameter(cmd, "Remove", DbType.Decimal, billPay.Remove);
                db.AddInParameter(cmd, "MemberCardNo", DbType.String, billPay.MemberCardNo);
                db.AddInParameter(cmd, "Cash", DbType.Decimal, billPay.Cash);
                db.AddInParameter(cmd, "CreditCard", DbType.Decimal, billPay.CreditCard);
                db.AddInParameter(cmd, "MemberCard", DbType.Decimal, billPay.MemberCard);
                db.AddInParameter(cmd, "Coupons", DbType.Decimal, billPay.Coupons);
                db.AddInParameter(cmd, "CouponsNo", DbType.String, billPay.CouponsNo);
                db.AddInParameter(cmd, "Remark", DbType.String, billPay.Remark);
                db.AddInParameter(cmd, "PayState", DbType.String, billPay.PayState);
                db.AddInParameter(cmd, "UserId", DbType.Guid, billPay.UserId);
                db.AddInParameter(cmd, "UserName", DbType.String, billPay.UserName);
                db.AddInParameter(cmd, "CreateDate", DbType.DateTime, billPay.CreateDate);
                db.AddInParameter(cmd, "Discount", DbType.Decimal, billPay.Discount);
                db.AddInParameter(cmd, "RstId", DbType.Guid, billPay.RstId);

                result = ExecSql(cmd) > 0;
            }
            catch(Exception ex)
            {
                Logger.Log(LoggingLevel.WxPay, ex);
            }

            return result;
        }

        public bool UpdateBillStateAsPaid(Guid billPayId, string payTransactionId)
        {
            bool result = false;

            try
            {
                DbCommand cmd = null;
                string remark = "微信支付订单流水号： " + payTransactionId;
                string sql = @"UPDATE [CrmRstCloud].[dbo].[OrderBillPay] SET PayState = @PayState, Remark = Remark+@Remark WHERE PayId = @PayId;";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "PayState", DbType.String, BillPayState.Paid);
                db.AddInParameter(cmd, "Remark", DbType.String, remark);
                db.AddInParameter(cmd, "PayId", DbType.Guid, billPayId);

                result = ExecSql(cmd) > 0;
            }
            catch(Exception ex)
            {
                Logger.Log(LoggingLevel.WxPay, ex);
            }

            return result;
        }

    }

    public class BillPayState
    {
        /// <summary>
        /// 订单已支付成功
        /// </summary>
        public const string Paid = "0x01";
        /// <summary>
        /// 订单还未支付
        /// </summary>
        public const string NotPaid = "0x02";
    }
}