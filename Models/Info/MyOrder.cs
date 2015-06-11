using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class MyOrder
    {
        public int PersonCount { get; set; }
        public string name { get; set; }
        public DateTime DiningDate { get; set;}
        public Guid OrderId { get; set; }
        public bool status { get; set; }
        public string  RstType { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

    }
    
}