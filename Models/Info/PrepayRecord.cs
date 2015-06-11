using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class PrepayRecord
    {
        ///<summary>
        ///流水号
        ///</summary>
        public int RecordId { get; set; }
        ///<summary>
        ///会员号
        ///</summary>
        public string Uid { get; set; }
        ///<summary>
        ///付款金额
        ///</summary>
        public Decimal? PrepayMoney { get; set; }
        ///<summary>
        ///赠送金额
        ///</summary>
        public Decimal? PresentMoney { get; set; }
        ///<summary>
        ///实际增加金额
        ///</summary>
        public Decimal? AddMoney { get; set; }
        ///<summary>
        ///预付时间
        ///</summary>
        public DateTime? PrepayDate { get; set; }
        ///<summary>
        ///活动编号
        ///</summary>
        public int? PromotionId { get; set; }
        ///<summary>
        ///代码（moneypaymodelcode):00现金01刷卡02微信支付03支付宝

        ///</summary>
        public string PayModel { get; set; }
        ///<summary>
        ///收银员
        ///</summary>
        public string UserId { get; set; }
        ///<summary>
        ///客户端提交的流水号
        ///</summary>
        public string SId { get; set; }
        ///<summary>
        ///
        ///</summary>
        public Guid BillPayId { get; set; }
        ///<summary>
        ///
        ///</summary>
        public Guid RstId { get; set; }
    }
}