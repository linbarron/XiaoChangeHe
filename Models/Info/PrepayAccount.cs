using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class PrepayAccount
    {
        public string uid { get; set; }
        public decimal AccountMoney { get; set; }
        public decimal PresentMoney { get; set; }

        public int PAId { get; set; }
        public Nullable<decimal> LastConsumeMoney { get; set; }
        public Nullable<System.DateTime> LastConsumeDate { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }
        public Nullable<decimal> LastPresentMoney { get; set; }
        public Nullable<decimal> TotalPresent { get; set; }
        
    }
}