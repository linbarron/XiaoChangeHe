using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class BillPay
    {
        /// <summary>
        /// 支付ID
        /// </summary>
        public Guid PayId { get; set; }
        public decimal Receivable { get; set; }
        public decimal PaidIn { get; set; }
        public decimal Change { get; set; }
        public decimal? Remove { get; set; }
        public string MemberCardNo { get; set; }
        /// <summary>
        /// 余额支付金额
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 在线支付金额
        /// </summary>
        public decimal CreditCard { get; set; }
        public decimal MemberCard { get; set; }
        public decimal Coupons { get; set; }
        public string CouponsNo { get; set; }
        public string Remark { get; set; }
        public string PayState { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }

        public decimal Discount { get; set; }

        public Guid RstId { get; set; }

    }
}