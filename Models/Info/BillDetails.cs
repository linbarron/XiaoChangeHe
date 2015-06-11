using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class BillDetails
    {
        public string DetailsId { get; set; }
        public string BillId { get; set; }
        public string ProductId { get; set; }
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string UseState { get; set; }
        public string SendState { get; set; }
        public string Remark { get; set; }
        public decimal Discount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsPay { get; set; }
        public string CostType { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? PayDate { get; set; }
    }
}
