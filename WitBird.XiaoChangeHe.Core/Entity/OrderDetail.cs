using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Entity
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }

        public string CustomName { get; set; }

        public string Telephone { get; set; }

        public List<Product> ProductList { get; set; }

        public DateTime EatTime { get; set; }

        public DateTime CreateTime { get; set; }

        public string Backlog { get; set; }
    }
}
