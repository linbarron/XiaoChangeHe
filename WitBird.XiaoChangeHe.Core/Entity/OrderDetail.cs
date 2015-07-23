using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Entity
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }

        public string ContactName { get; set; }

        public string Telephone { get; set; }

        public List<Product> ProductList { get; set; }

        public DateTime? DiningDate { get; set; }

        public decimal TotalMoney { get; set; }

        public DateTime CreateTime { get; set; }

        public string Backlog { get; set; }

        public int? PersonCount { get; set; }

        public string Status { get; set; }
    }
}
