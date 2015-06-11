﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class BillPay
    {
        public string PayId { get; set; }
        public decimal Receivable { get; set; }
        public decimal PaidIn { get; set; }
        public decimal Change { get; set; }
        public decimal? Remove { get; set; }
        public string MemberCardNo { get; set; }
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        public decimal MemberCard { get; set; }
        public decimal Coupons { get; set; }
        public string CouponsNo { get; set; }
        public string Remark { get; set; }
        public string PayState { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }

    }
}