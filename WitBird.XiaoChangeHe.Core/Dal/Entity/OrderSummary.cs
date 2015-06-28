using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal.Entity
{
    public class OrderSummary
    {
        public Guid OrderId { get; set; }

        public Guid RestaurantId { get; set; }

        public string RestaurantName { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public decimal TotalMoney { get; set; }

        public bool Status { get; set; }

        public DateTime? DiningDate { get; set; }

        public DateTime CreateTime { get; set; }

        public string Backlog { get; set; }
    }
}
