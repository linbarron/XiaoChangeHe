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
    public class BillPayModel : DbHelper
    {
        /// <summary>
        /// 查询支付订单
        /// </summary>
        /// <param name="payId"></param>
        /// <returns></returns>
        public BillPay GetBillPayById(Guid payId)
        {
            BillPay billPay = null;

            try
            {
                string sql = @"select * from BillPay where PayId=@PayId";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "PayId", DbType.String, payId.ToString());

                var reader = db.ExecuteReader(cmd);

                while (reader.Read())
                {
                    billPay = new BillPay()
                    {
                        PayId = payId,
                        Cash = reader.TryGetValue("Cash", 0),
                        Change = reader.TryGetValue("Change", 0),
                        Coupons = reader.TryGetValue("Change", 0),
                        CouponsNo = reader.TryGetValue("Change", ""),
                        CreateDate = reader.TryGetValue("Change", DateTime.Now),
                        CreditCard = reader.TryGetValue("Change", 0),
                        MemberCard = reader.TryGetValue("Change", 0),
                        MemberCardNo = reader.TryGetValue("Change", ""),
                        PaidIn = reader.TryGetValue("Change", 0),
                        PayState = reader.TryGetValue("Change", BillPayState.NotPaid),
                        Receivable = reader.TryGetValue("Change", 0),
                        Remark = reader.TryGetValue("Change", ""),
                        Remove = reader.TryGetValue("Change", 0),
                        UserId = reader.TryGetValue("Change", Guid.Empty),
                        UserName = reader.TryGetValue("Change", ""),
                        RstId = reader.TryGetValue("RstId", Guid.Empty),
                        Discount = reader.TryGetValue("Discount", 0)
                    };
                }
            }
            catch
            {

            }

            return billPay;
        }

        /// <summary>
        /// 增加支付订单记录
        /// </summary>
        /// <param name="billPay"></param>
        /// <returns></returns>
        public bool AddBillPay(BillPay billPay)
        {
            bool result = false;

            try
            {
                DbCommand cmd = null;
                string sql = @"
                            INSERT INTO [CrmRstCloud].[dbo].[BillPay]
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
            catch
            {

            }

            return result;
        }

        public bool UpdateBillStateAsPaid(Guid billPayId)
        {
            bool result = false;

            try
            {
                DbCommand cmd = null;
                string sql = @"UPDATE [CrmRstCloud].[dbo].[BillPay] SET PayState = @PayState WHERE PayId = @PayId;";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "PayState", DbType.String, BillPayState.Paid);
                db.AddInParameter(cmd, "PayId", DbType.Guid, billPayId);

                result = ExecSql(cmd) > 0;
            }
            catch
            {

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