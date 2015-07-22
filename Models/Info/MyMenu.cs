using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class MyMenu
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MemberPrice { get; set; }
        public string UseState { get; set; }
        public int totalcount { get; set; }
        public int ProductCount { get; set; }
        public Guid ProductId { get; set; }
        public DateTime DiningDate { get; set; }
        public string status { get; set; }
        
    }
}