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
    public class PrepayRecordModel : DbHelper
    {

        #region getPrepayRecordListInfoData


        private class getPrepayRecordListInfoDataParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "Uid";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }

        #endregion

        public List<PrepayRecord> getConsumptionRecordsListInfoData(string Uid)
        {
            List<PrepayRecord> list = null;
            try
            {
                if (string.IsNullOrEmpty(Uid))
                {
                    return null;
                }
                IParameterMapper ipmapper = new getPrepayRecordListInfoDataParameterMapper();
                DataAccessor<PrepayRecord> tableAccessor;
                //                string strSql = @"select p.AddMoney,p.BillPayId,p.PayModel,p.PrepayDate,p.PrepayMoney,p.PresentMoney,p.PromotionId,p.RecordId,p.RstId,p.SId,p.Uid,p.UserId 
                //                                from PrepayRecord p where p.Uid=@Uid and  p.PrepayMoney<0  and p.PrepayDate between  dateadd (MM,-1,GETDATE()) and   getdate() order by p.PrepayDate desc";

                string strSql = @"
                                select top 10 *
                                from PrepayRecord p
                                left join BillPay b on b.PayId = p.BillPayId
                                where p.Uid=@Uid and  p.PrepayMoney<0   and b.PayState = '0x01'--0x01:支付成功 
                                order by p.PrepayDate desc";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<PrepayRecord>.MapAllProperties()
                     .Map(t => t.Uid).ToColumn("Uid")
                     .Map(t => t.AddMoney).ToColumn("AddMoney")
                     .Map(t => t.PayModel).ToColumn("PayModel")
                     .Map(t => t.PrepayDate).ToColumn("PrepayDate")
                     .Map(t => t.PrepayMoney).ToColumn("PrepayMoney")
                     .Map(t => t.PresentMoney).ToColumn("PresentMoney")
                     .Map(t => t.PromotionId).ToColumn("PromotionId")
                     .Map(t => t.RecordId).ToColumn("RecordId")
                     .Map(t => t.UserId).ToColumn("UserId")
                     .Map(t => t.BillPayId).ToColumn("BillPayId")
                     .Map(t => t.RstId).ToColumn("RstId")
                     .Map(t => t.SId).ToColumn("SId")
                    .Build());
                list = tableAccessor.Execute(new string[] { Uid }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<PrepayRecord> getRechargeRecordListInfoData(string Uid)
        {
            List<PrepayRecord> list = null;
            try
            {
                if (string.IsNullOrEmpty(Uid))
                {
                    return null;
                }
                IParameterMapper ipmapper = new getPrepayRecordListInfoDataParameterMapper();
                DataAccessor<PrepayRecord> tableAccessor;
                //string strSql = @"select p.AddMoney,p.BillPayId,p.PayModel,p.PrepayDate,p.PrepayMoney,p.PresentMoney,p.PromotionId,p.RecordId,p.RstId,p.SId,p.Uid,p.UserId 
                //                from PrepayRecord p where p.Uid=@Uid and  p.PrepayMoney>0 and p.PrepayDate between  dateadd (MM,-1,GETDATE()) and   getdate() order by p.PrepayDate desc";

                string strSql = @"
                                select top 10 p.* from PrepayRecord p
                                left join BillPay b on b.PayId = p.BillPayId
                                where p.Uid=@Uid and  p.PrepayMoney>0  and b.PayState = '0x01'--0x01:支付成功
                                order by p.PrepayDate desc";

                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<PrepayRecord>.MapAllProperties()
                     .Map(t => t.Uid).ToColumn("Uid")
                     .Map(t => t.AddMoney).ToColumn("AddMoney")
                     .Map(t => t.PayModel).ToColumn("PayModel")
                     .Map(t => t.PrepayDate).ToColumn("PrepayDate")
                     .Map(t => t.PrepayMoney).ToColumn("PrepayMoney")
                     .Map(t => t.PresentMoney).ToColumn("PresentMoney")
                     .Map(t => t.PromotionId).ToColumn("PromotionId")
                     .Map(t => t.RecordId).ToColumn("RecordId")
                     .Map(t => t.UserId).ToColumn("UserId")
                     .Map(t => t.BillPayId).ToColumn("BillPayId")
                     .Map(t => t.RstId).ToColumn("RstId")
                     .Map(t => t.SId).ToColumn("SId")
                    .Build());
                list = tableAccessor.Execute(new string[] { Uid }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool AddPrepayRecord(PrepayRecord prepayRecord)
        {
            bool result = false;

            try
            {
                DbCommand cmd = null;
                string sql = @"
                INSERT INTO [CrmRstCloud].[dbo].[PrepayRecord]
                           ([Uid]
                           ,[PrepayMoney]
                           ,[PresentMoney]
                           ,[AddMoney]
                           ,[PrepayDate]
                           ,[PromotionId]
                           ,[PayModel]
                           ,[UserId]
                           ,[SId]
                           ,[BillPayId]
                           ,[RstId]
                           ,[ScoreVip]
                           ,[PayByScore]
                           ,[RState]
                           ,[AsureDate]
                           ,[RecMoney]
                           ,[DiscountlMoeny])
                     VALUES
                           (@Uid
                           ,@PrepayMoney
                           ,@PresentMoney
                           ,@AddMoney
                           ,@PrepayDate
                           ,@PromotionId
                           ,@PayModel
                           ,@UserId
                           ,@SId
                           ,@BillPayId
                           ,@RstId
                           ,@ScoreVip
                           ,@PayByScore
                           ,@RState
                           ,@AsureDate
                           ,@RecMoney
                           ,@DiscountlMoeny)
                ";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "Uid", DbType.String, prepayRecord.Uid);
                db.AddInParameter(cmd, "PrepayMoney", DbType.Decimal, prepayRecord.PrepayMoney);
                db.AddInParameter(cmd, "PresentMoney", DbType.Decimal, prepayRecord.PresentMoney);
                db.AddInParameter(cmd, "AddMoney", DbType.Decimal, prepayRecord.AddMoney);
                db.AddInParameter(cmd, "PrepayDate", DbType.DateTime, prepayRecord.PrepayDate);
                db.AddInParameter(cmd, "PromotionId", DbType.Int32, prepayRecord.PromotionId);
                db.AddInParameter(cmd, "PayModel", DbType.String, prepayRecord.PayModel);
                db.AddInParameter(cmd, "UserId", DbType.String, prepayRecord.UserId);
                db.AddInParameter(cmd, "SId", DbType.String, prepayRecord.SId);
                db.AddInParameter(cmd, "BillPayId", DbType.Guid, prepayRecord.BillPayId);
                db.AddInParameter(cmd, "RstId", DbType.Guid, prepayRecord.RstId);
                db.AddInParameter(cmd, "ScoreVip", DbType.Int32, prepayRecord.ScoreVip);
                db.AddInParameter(cmd, "PayByScore", DbType.Int32, prepayRecord.PayByScore);
                db.AddInParameter(cmd, "RState", DbType.String, prepayRecord.RState);
                db.AddInParameter(cmd, "AsureDate", DbType.DateTime, prepayRecord.AsureDate);
                db.AddInParameter(cmd, "RecMoney", DbType.Decimal, prepayRecord.RecMoney);
                db.AddInParameter(cmd, "DiscountlMoeny", DbType.Decimal, prepayRecord.DiscountlMoeny);

                result = ExecSql(cmd) > 0;
            }
            catch
            {

            }

            return result;
        }

        public PrepayRecord GetPrepayRecordByBillPayId(Guid billPayId)
        {
            PrepayRecord prepayRecord = null;

            try
            {
                string sql = @"select top 1 * from PrepayRecord where BillPayId=@BillPayId;";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "BillPayId", DbType.Guid, billPayId);

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        prepayRecord = new PrepayRecord()
                        {
                            AddMoney = reader.TryGetValue<Decimal?>("AddMoney"),
                            AsureDate = reader.TryGetValue<DateTime?>("AsureDate"),
                            BillPayId = reader.TryGetValue<Guid?>("BillPayId"),
                            DiscountlMoeny = reader.TryGetValue<Decimal?>("DiscountlMoeny"),
                            PayByScore = reader.TryGetValue<Int32?>("PayByScore"),
                            PayModel = reader.TryGetValue<String>("PayModel"),
                            PrepayDate = reader.TryGetValue<DateTime?>("PrepayDate"),
                            PrepayMoney = reader.TryGetValue<Decimal?>("PrepayMoney"),
                            PresentMoney = reader.TryGetValue<Decimal?>("PresentMoney"),
                            PromotionId = reader.TryGetValue<Int32?>("PromotionId"),
                            RecMoney = reader.TryGetValue<Decimal?>("RecMoney"),
                            RecordId = reader.TryGetValue<Int32>("RecordId"),
                            RState = reader.TryGetValue<String>("RState"),
                            RstId = reader.TryGetValue<Guid?>("RstId"),
                            ScoreVip = reader.TryGetValue<Int32?>("ScoreVip"),
                            SId = reader.TryGetValue<String>("SId"),
                            Uid = reader.TryGetValue<String>("Uid"),
                            UserId = reader.TryGetValue<String>("UserId")
                        };
                    }
                }
            }
            catch
            {
                throw;
            }

            return prepayRecord;
        }

        public PrepayRecord GetPrepayRecordByOrderId(string orderId)
        {
            PrepayRecord prepayRecord = null;

            try
            {
                string sql = @"select top 1 * from PrepayRecord where SId=@orderId;";
                DbCommand cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "orderId", DbType.String, orderId.ToString());

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        prepayRecord = new PrepayRecord()
                        {
                            AddMoney = reader.TryGetValue<Decimal?>("AddMoney"),
                            AsureDate = reader.TryGetValue<DateTime?>("AsureDate"),
                            BillPayId = reader.TryGetValue<Guid?>("BillPayId"),
                            DiscountlMoeny = reader.TryGetValue<Decimal?>("DiscountlMoeny"),
                            PayByScore = reader.TryGetValue<Int32?>("PayByScore"),
                            PayModel = reader.TryGetValue<String>("PayModel"),
                            PrepayDate = reader.TryGetValue<DateTime?>("PrepayDate"),
                            PrepayMoney = reader.TryGetValue<Decimal?>("PrepayMoney"),
                            PresentMoney = reader.TryGetValue<Decimal?>("PresentMoney"),
                            PromotionId = reader.TryGetValue<Int32?>("PromotionId"),
                            RecMoney = reader.TryGetValue<Decimal?>("RecMoney"),
                            RecordId = reader.TryGetValue<Int32>("RecordId"),
                            RState = reader.TryGetValue<String>("RState"),
                            RstId = reader.TryGetValue<Guid?>("RstId"),
                            ScoreVip = reader.TryGetValue<Int32?>("ScoreVip"),
                            SId = reader.TryGetValue<String>("SId"),
                            Uid = reader.TryGetValue<String>("Uid"),
                            UserId = reader.TryGetValue<String>("UserId")
                        };
                    }
                }
            }
            catch
            {
                throw;
            }

            return prepayRecord;
        }
    }
}