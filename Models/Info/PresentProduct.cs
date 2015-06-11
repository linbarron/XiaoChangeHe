using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class PresentProduct
    {
        public Guid ProductId { get; set; }
        //public Guid TypeId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string MinUnit { get; set; }
        public Guid RstId { get; set; }
       // public bool Status { get; set; }
        public string CodeTypeListName { get; set; }
        public int ProductCount { get; set; }

        
    }
}