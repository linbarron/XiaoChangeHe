using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Entity
{
    public class Product
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int Count { get; set; }

        public byte[] Image { get; set; }
    }
}
