using System;
using System.Collections.Generic;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class CrmMember
    {
        public string Uid { get; set; }
        public string MemberName { get; set; }
        public string Addr { get; set; }
        public string Tel { get; set; }
        public string MemberSource { get; set; }
        public string SourceAccountId { get; set; }
        public string Password { get; set; }
        public string Idcard { get; set; }
        public DateTime Birthday { get; set; }
        public int? TypeId { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string UseState { get; set; }
        public bool? Sex { get; set; }
        public Guid? CompanyId { get; set; }
        public string TypeName { get; set; }
    }
}