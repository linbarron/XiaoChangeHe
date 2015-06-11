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
    public class CrmMemberScoreModel : DbHelper
    {
        #region SelCrmMemberScoreInfo


        private class SelCrmMemberScoreInfoParameterMapper : IParameterMapper
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
        public List<CrmMemberScore> SelCrmMemberScoreInfo(string Uid)
        {
            List<CrmMemberScore> list = null;
            try
            {
                IParameterMapper ipmapper = new SelCrmMemberScoreInfoParameterMapper();
                DataAccessor<CrmMemberScore> tableAccessor;
                string strSql = @"select  n.TotalScore,n.LastScore,n.LastScoredDate,n.Score ,n.UseMoney, n.UseScore from CrmMemberScore n
                    where n.Uid=@Uid";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<CrmMemberScore>.MapAllProperties()
                   // .Map(t => t.Uid).ToColumn("Uid")
                    .Map(t => t.TotalScore).ToColumn("TotalScore")
                    .Map(t => t.LastScore).ToColumn("LastScore")
                    .Map(t => t.LastScoredDate).ToColumn("LastScoredDate")
                    .Map(t => t.Score).ToColumn("Score")
                    .Map(t => t.UseMoney).ToColumn("UseMoney")
                    .Map(t => t.UseScore).ToColumn("UseScore")
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
    }
}