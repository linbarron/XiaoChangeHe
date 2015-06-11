using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class Bill
    {
        public string BillId { get; set; }
        public string OrderId { get; set; }
        public DateTime BeginDate { get; set; }
        public string TableId { get; set; }
        public bool? Status { get; set; }
        public string PayId { get; set; }
    }
}
