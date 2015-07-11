using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.SubmitOrder
{
    public class SubmitOrderEntity
    {
        public string SourceAccountId { get; set; }
        public DateTime DiningDate { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public int? PersonCount { get; set; }
        public Guid OrderId { get; set; }
        public int? sex { set; get; }
    }
}