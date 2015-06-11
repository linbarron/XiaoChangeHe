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
    public class AutoMenuConfigurationModel : DbHelper
    {
        #region getAutoProduct


        private class getAutoProductParameterMapper : IParameterMapper
        {
            #region IParameterMapper 成员

            public void AssignParameters(System.Data.Common.DbCommand command, object[] parameterValues)
            {
                DbParameter ps0 = command.CreateParameter();
                ps0.ParameterName = SqlPara + "RestaurantId";
                ps0.DbType = DbType.String;
                ps0.Value = parameterValues[0];
                command.Parameters.Add(ps0);
                DbParameter ps1 = command.CreateParameter();
                ps1.ParameterName = SqlPara + "PeopleCount";
                ps1.DbType = DbType.String;
                ps1.Value = parameterValues[1];
                command.Parameters.Add(ps1);
            }
            #endregion


        }

      
        #endregion

    }
}