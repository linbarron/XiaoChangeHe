using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class ProductisBook
    {
        public Guid productId { get; set; }
        public Boolean isBook { get; set; }
    }

    public class ProductTypeIsBook {

        public Guid productTypeId { get; set; }
        public Boolean isBook { get;set; }
    }

}