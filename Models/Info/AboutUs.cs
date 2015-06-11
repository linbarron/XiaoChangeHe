using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class AboutUs
    {
        public Guid ID{ get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime DateTime { get; set; }
        public Guid CompanyId { get; set; }
       
    }
}