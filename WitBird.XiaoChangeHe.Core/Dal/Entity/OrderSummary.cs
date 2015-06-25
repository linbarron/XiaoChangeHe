using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal.Entity
{
    public class OrderSummary
    {
        public Guid OrderId { get; set; }

        public string CustomName { get; set; }

        public string Telephone { get; set; }

        public DateTime? DiningDate { get; set; }

        public DateTime CreateTime { get; set; }

        public string Backlog { get; set; }
    }
}
