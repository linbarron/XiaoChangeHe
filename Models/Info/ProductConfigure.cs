using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class ProductConfigure
    {
        public Guid ProductId{get;set;}
        public int Count { get; set; }
        public int LoveCount { get; set; }

    }
}