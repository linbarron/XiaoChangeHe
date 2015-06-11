using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class AutoMenus
    {
        public Guid ID { get; set; }
        public bool? Status { get; set; }
        public DateTime Datatime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
       
    }
    public class AutoMenusProduct {
        public Guid AID { get; set; }
        public Guid ProductId { get; set; }
        public Guid RestaurantId { get; set; }
        public bool? Status { get; set; }
        public DateTime Datatime { get; set; }
        public string type { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public int Hot { get; set; }
        public int Popular { get; set; }

    
    }

    public class AutoMenuAndProduct {
        public Guid ID { get; set; }
        public bool? Status { get; set; }
        public DateTime Datatime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid AID { get; set; }
        public Guid ProductId { get; set; }
        public Guid RestaurantId { get; set; }
        public bool? AStatus { get; set; }
       
        public string type { get; set; }
        public string ADescription { get; set; }
        public int OrderId { get; set; }
        public int Hot { get; set; }
        public int Popular { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    
    }
}