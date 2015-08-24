using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models
{
    public static class Constants
    {
        public static string companyId = ConfigurationManager.AppSettings["CompanyId"];
        public static string HostDomain = ConfigurationManager.AppSettings["HostDomain"];

        public static Guid CompanyId
        {
            get
            {
                Guid result;
                if (!string.IsNullOrWhiteSpace(companyId) && 
                    Guid.TryParse(companyId, out result))
                {
                    return result;
                }

                return Guid.Empty;
            }
        }
    }
}