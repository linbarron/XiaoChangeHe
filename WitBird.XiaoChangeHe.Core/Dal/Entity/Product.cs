using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal.Entity
{
    public class Product
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public byte[] Image { get; set; }
    }
}
