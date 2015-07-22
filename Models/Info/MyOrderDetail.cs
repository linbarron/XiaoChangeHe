using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class MyOrderDetail
    {
        public DateTime DiningDate { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
       public int? PersonCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MemberPrice { get; set; }
        public string Remark { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
        public string RstName { get; set; }
        public string address { get; set; }
        public string RstPhone { get; set; }
        public Guid OrderId { get; set; }
        public decimal total { get; set; }
        public Guid proId { get; set; }
        public bool status { get; set; }
        public string OrderStaus { get; set; }
        public string CodeTypeListName { get; set; }
        public string  RstType{get;set;}
        public string UseState { get; set; }
    }
}