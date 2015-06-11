using System;
using System.Collections.Generic;
using System.Text;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class OrderDetails
    {
        public Guid DetailsId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string UseState { get; set; }
        public string SendState { get; set; }
        public string Remark { get; set; }

        public decimal Discount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime CreateDate { get; set; } 

    }
}
