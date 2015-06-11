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
                string strSql = @"select p.AccountMoney, p.uid,p.PresentMoney from PrepayAccount p where p.uid=@SourceAccountId";
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

        #endregion
        }
    }
}