using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Models
{
    public class CrmMemberModel : DbHelper
    {
        #region getCrmMemberListInfoData


        private class getCrmMemberListInfoDataParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "SourceAccountId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
            }
            #endregion

        }
        public List<CrmMember> getCrmMemberListInfoData(string SourceAccountId)
        {
            List<CrmMember> list = null;
            try
            {
                if (string.IsNullOrEmpty(SourceAccountId))
                {
                    return null;
                }
                IParameterMapper ipmapper = new getCrmMemberListInfoDataParameterMapper();
                DataAccessor<CrmMember> tableAccessor;
                string strSql = @"select a.Uid,a.MemberName,a.Addr,a.Tel,a.MemberSource,a.SourceAccountId,
a.Password,a.Idcard,a.Birthday,a.TypeId,a.RegDate,a.ExpiredDate,a.UseState,a.Sex
,a.CompanyId,b.TypeName from CrmMember a,CrmMemberType b where a.TypeId=b.TypeId and a.SourceAccountId=@SourceAccountId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<CrmMember>.MapAllProperties()
                     .Map(t => t.Uid).ToColumn("Uid")
                     .Map(t => t.MemberName).ToColumn("MemberName")
                     .Map(t => t.Addr).ToColumn("Addr")
                     .Map(t => t.Tel).ToColumn("Tel")
                     .Map(t => t.MemberSource).ToColumn("MemberSource")
                     .Map(t => t.SourceAccountId).ToColumn("SourceAccountId")
                     .Map(t => t.Password).ToColumn("Password")
                     .Map(t => t.Idcard).ToColumn("Idcard")
                     .Map(t => t.Birthday).ToColumn("Birthday")
                     .Map(t => t.TypeId).ToColumn("TypeId")
                     .Map(t => t.RegDate).ToColumn("RegDate")
                     .Map(t => t.ExpiredDate).ToColumn("ExpiredDate")
                     .Map(t => t.UseState).ToColumn("UseState")
                     .Map(t => t.Sex).ToColumn("Sex")
                     .Map(t => t.CompanyId).ToColumn("CompanyId")
                     .Map(t => t.TypeName).ToColumn("TypeName")
                    .Build());
                list = tableAccessor.Execute(new string[] { SourceAccountId }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public List<PrepayAccount> getPrepayAccount(string memberId)
        {
            List<PrepayAccount> list = null;
            try
            {
                if (string.IsNullOrEmpty(memberId))
                {
                    return null;
                }
                IParameterMapper ipmapper = new getCrmMemberListInfoDataParameterMapper();
                DataAccessor<PrepayAccount> tableAccessor;
                string strSql = @"select * from PrepayAccount p where p.uid=@SourceAccountId";
                tableAccessor = db.CreateSqlStringAccessor(strSql, ipmapper, MapBuilder<PrepayAccount>.MapAllProperties()
                     .Map(t => t.uid).ToColumn("Uid")
                     .Map(t => t.AccountMoney).ToColumn("AccountMoney")
                     .Map(t => t.PresentMoney).ToColumn("PresentMoney")

                    .Build());
                list = tableAccessor.Execute(new string[] { memberId }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdatePrepayAccount(PrepayAccount prepayAccount)
        {
            bool result = false;

            try
            {
                DbCommand cmd = null; 

                string sql = @"UPDATE [CrmRstCloud].[dbo].[PrepayAccount] SET AccountMoney = @AccountMoney, 
PresentMoney = @PresentMoney, TotalPresent = @TotalPresent, TotalMoney=@TotalMoney, LastPresentMoney=@LastPresentMoney, 
LastConsumeMoney = @LastConsumeMoney WHERE Uid = @Uid;";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "AccountMoney", DbType.Decimal, prepayAccount.AccountMoney);
                db.AddInParameter(cmd, "PresentMoney", DbType.Decimal, prepayAccount.PresentMoney);
                db.AddInParameter(cmd, "TotalPresent", DbType.Decimal, prepayAccount.TotalPresent);
                db.AddInParameter(cmd, "TotalMoney", DbType.Decimal, prepayAccount.TotalMoney);
                db.AddInParameter(cmd, "LastConsumeMoney", DbType.Decimal, prepayAccount.LastConsumeMoney);
                db.AddInParameter(cmd, "LastPresentMoney", DbType.Decimal, prepayAccount.LastPresentMoney);
                db.AddInParameter(cmd, "Uid", DbType.String, prepayAccount.uid);

                result = ExecSql(cmd) > 0;
            }
            catch
            {
                throw;
            }

            return result;

        }

        #region Save
        //public int SaveOrderDetails(string type, OrderDetails info)
        public int Save(string id, string name, string phone, string sex, string bir, string addr)
        {

            {


                DbCommand cmd = null;
                string sql;
                try
                {

                    sql = "UPDATE CrmMember set MemberName= @name,Tel= @phone,sex=@sex,addr=@addr,Birthday=@bir  WHERE uid=@id";
                    cmd = db.GetSqlStringCommand(sql);
                    //db.AddInParameter(cmd, "DetailsId", DbType.Guid, info.DetailsId);
                    db.AddInParameter(cmd, "name", DbType.String, name);
                    db.AddInParameter(cmd, "phone", DbType.String, phone);
                    db.AddInParameter(cmd, "sex", DbType.Boolean, sex);
                    db.AddInParameter(cmd, "addr", DbType.String, addr);
                    db.AddInParameter(cmd, "bir", DbType.DateTime, bir);
                    db.AddInParameter(cmd, "id", DbType.String, id);

                    return ExecSql(cmd);
                }
                catch (Exception)
                {
                }
                return 0;
            }
        }

        public bool SaveMember(string wxOpenid, string companyid)
        {
            bool resukt = false;
            using (TransactionScope transaction = new TransactionScope())
            {
                var user = getCrmMemberListInfoData(wxOpenid);
                if (user == null || (user != null && user.Count == 0))
                {
                    CrmMember member = new CrmMember();
                    member.MemberSource = "01";
                    member.SourceAccountId = wxOpenid;
                    member.TypeId = 1;
                    member.UseState = "01";
                    member.RegDate = DateTime.Now;
                    member.CompanyId = new Guid(companyid);

                    var v = GetNewUserId();
                    member.Uid = v.ToString();

                    #region Add new user
                    DbCommand cmd = null;
                    string sql;
                    sql = "insert into CrmMember([Uid],MemberSource,SourceAccountId,TypeId,UseState,RegDate,CompanyId) " +
                          "values(@Uid,@MemberSource,@SourceAccountId,@TypeId,@UseState,@RegDate,@CompanyId)";
                    cmd = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmd, "Uid", DbType.String, member.Uid);
                    db.AddInParameter(cmd, "MemberSource", DbType.String, member.MemberSource);
                    db.AddInParameter(cmd, "SourceAccountId", DbType.String, member.SourceAccountId);
                    db.AddInParameter(cmd, "TypeId", DbType.Int32, member.TypeId);
                    db.AddInParameter(cmd, "UseState", DbType.String, member.UseState);
                    db.AddInParameter(cmd, "RegDate", DbType.DateTime, member.RegDate);
                    db.AddInParameter(cmd, "CompanyId", DbType.Guid, member.CompanyId);

                    ExecSql(cmd);

                    #endregion

                    #region Add PrepayAccount

                    PrepayAccount acc = new PrepayAccount();
                    acc.uid = member.Uid;
                    acc.AccountMoney = 0;
                    acc.TotalMoney = 0;
                    acc.PresentMoney = 0;
                    acc.TotalPresent = 0;
                    acc.LastConsumeMoney = 0;
                    acc.LastPresentMoney = 0;

                    cmd = null;
                    sql = "insert into PrepayAccount([Uid],AccountMoney,TotalMoney,PresentMoney,TotalPresent,LastConsumeMoney,LastPresentMoney) " +
                          "values(@Uid,@AccountMoney,@TotalMoney,@PresentMoney,@TotalPresent,@LastConsumeMoney,@LastPresentMoney)";
                    cmd = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmd, "Uid", DbType.String, acc.uid);
                    db.AddInParameter(cmd, "AccountMoney", DbType.Decimal, acc.AccountMoney);
                    db.AddInParameter(cmd, "TotalMoney", DbType.Decimal, acc.TotalMoney);
                    db.AddInParameter(cmd, "PresentMoney", DbType.Decimal, acc.PresentMoney);
                    db.AddInParameter(cmd, "TotalPresent", DbType.Decimal, acc.TotalPresent);
                    db.AddInParameter(cmd, "LastConsumeMoney", DbType.Decimal, acc.LastConsumeMoney);
                    db.AddInParameter(cmd, "LastPresentMoney", DbType.Decimal, acc.LastPresentMoney);

                    ExecSql(cmd);

                    #endregion

                    #region Add CrmMemberScore

                    CrmMemberScore scroe = new CrmMemberScore()
                    {
                        LastScore = 0,
                        LastScoredDate = DateTime.Now,
                        Score = 0,
                        TotalScore = 0,
                        Uid = member.Uid,
                        UseMoney = 0,
                        UseScore = 0
                    };
                    cmd = null;
                    sql = "insert into CrmMemberScore([Uid],LastScoredDate,Score,LastScore,UseMoney,UseScore) " +
                          "values(@Uid,@LastScoredDate,@Score,@LastScore,@UseMoney,@UseScore)";
                    cmd = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmd, "Uid", DbType.String, scroe.Uid);
                    db.AddInParameter(cmd, "LastScoredDate", DbType.DateTime, scroe.LastScoredDate);
                    db.AddInParameter(cmd, "Score", DbType.Int32, scroe.Score);
                    db.AddInParameter(cmd, "LastScore", DbType.Int32, scroe.LastScore);
                    db.AddInParameter(cmd, "UseMoney", DbType.Int32, scroe.UseMoney);
                    db.AddInParameter(cmd, "UseScore", DbType.Int32, scroe.UseScore);

                    ExecSql(cmd);

                    #endregion
                }
                else
                {
                    #region Set user to New
                    DbCommand cmd = null;
                    string sql;
                    sql = "update CrmMember set UseState=1 where SourceAccountId=@SourceAccountId";
                    cmd = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmd, "SourceAccountId", DbType.String, wxOpenid);

                    ExecSql(cmd);

                    #endregion
                }
                transaction.Complete();
                resukt = true;
            }
            return resukt;
        }

        //public bool SaveMemberInfo(string wxOpenid, string companyid, string province, string city, string nickname, string sex)
        //{
        //    try
        //    {
        //        CrmMember member = db.CrmMember.Single(t => t.SourceAccountId == wxOpenid && t.CompanyId == new Guid(companyid));
        //        member.ProvinceId = province;
        //        member.CityId = city;
        //        if (member.MemberName == null)
        //        {
        //            member.MemberName = nickname;
        //            if (member.Sex == null)
        //            {
        //                if (sex == "1")
        //                {
        //                    member.Sex = true;
        //                }
        //                else if (sex == "2")
        //                {
        //                    member.Sex = false;
        //                }
        //            }
        //        }
        //        return db.SaveChanges() > 0;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public bool DiscardMember(string wxOpenId, string companyid)
        {
            #region Set user to 99

            DbCommand cmd = null;
            string sql;
            sql = "update CrmMember set UseState=99 where SourceAccountId=@SourceAccountId";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "SourceAccountId", DbType.String, wxOpenId);

            return ExecSql(cmd) > 0;

            #endregion
        }


        /// <summary>
        /// 分配新用户ID
        /// </summary>
        /// <returns></returns>
        public long GetNewUserId()
        {
            long uId = 1;
            try
            {
                DbCommand cmd = null;
                string sql;
                sql = "select max(uid) from CrmMember;";
                cmd = db.GetSqlStringCommand(sql);
                var result = ExecuteScalar(cmd);
                if (result != null)
                {
                    uId = Convert.ToInt64(result);
                    uId++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uId;
        }

        #endregion
    }
}