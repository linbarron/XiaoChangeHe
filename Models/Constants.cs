using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models
{
    public static class Constants
    {
        public static Guid CompanyId = Guid.Parse("CB824E58-E2CA-4C95-827A-CA62D528C6A7");
        public static string HostDomain = ConfigurationManager.AppSettings["HostDomain"];
    }
}