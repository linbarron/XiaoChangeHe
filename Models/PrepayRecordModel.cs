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
                string strSql = @"select p.AddMoney,p.BillPayId,p.PayModel,p.PrepayDate,p.PrepayMoney,p.PresentMoney,p.PromotionId,p.RecordId,p.RstId,p.SId,p.Uid,p.UserId 
                                from PrepayRecord p where p.Uid=@Uid and  p.PrepayMoney<0  and p.PrepayDate between  dateadd (MM,-1,GETDATE()) and   getdate() order by p.PrepayDate desc";
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
        #endregion


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
                string strSql = @"select p.AddMoney,p.BillPayId,p.PayModel,p.PrepayDate,p.PrepayMoney,p.PresentMoney,p.PromotionId,p.RecordId,p.RstId,p.SId,p.Uid,p.UserId 
                                from PrepayRecord p where p.Uid=@Uid and  p.PrepayMoney>0 and p.PrepayDate between  dateadd (MM,-1,GETDATE()) and   getdate() order by p.PrepayDate desc";
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
        
    }
}