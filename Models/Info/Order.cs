using System;
using System.Collections.Generic;
using System.Text;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public DateTime DiningDate { get; set; }
        public int? PersonCount { get; set; }
        public int? TableCount { get; set; }
        public string MemberCardNo { get; set; }
        public string ReserveType { get; set; }
        public decimal PrepayPrice { get; set; }

        public string Remark { get; set; }
        public bool? Status { get; set; }
        public Guid OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime CreateDate { get; set; }
        public Boolean Sex { get; set; }
        public Guid RstId { get; set; }
    }

    public class FastFoodOrder
    {
        public Guid Id { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public DateTime DiningDate { get; set; }
        public int? PersonCount { get; set; }
        public int? TableCount { get; set; }
        public string MemberCardNo { get; set; }
        public string ReserveType { get; set; }
        public decimal PrepayPrice { get; set; }

        public string Remark { get; set; }
        public bool? Status { get; set; }
        public Guid OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid RstId { get; set; }
        public string RstType { get; set; }
    }
}
