using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal.Entity
{
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
