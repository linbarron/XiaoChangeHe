using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangeHe.Core
{
    /// <summary>
    /// 订单所有状态定义。
    /// </summary>
    public class OrderStatus
    {
        /// <summary>
        /// 新订单，未付款状态。该状态允许用户取消订单
        /// </summary>
        public const string New = "New";

        /// <summary>
        /// 订单已支付。该状态允许用户申请退款
        /// </summary>
        public const string Paid = "Paid";

        /// <summary>
        /// 订单已被服务员确认，出菜完成。该状态不允许其他任何操作
        /// </summary>
        public const string Confirmed = "Confirmed";

        /// <summary>
        /// 订单配送完毕，已完成。该状态不允许其他任何操作
        /// </summary>
        public const string Complete = "Complete";

        /// <summary>
        /// 订单被主动取消，只有处于New状态未支付订单允许被取消。该状态不允许其他任何操作
        /// </summary>
        public const string Cancelled = "Cancelled";

        /// <summary>
        /// 订单被申请退款，只有处于Paid已付款状态允许申请退款。该状态不允许其他任何操作
        /// </summary>
        public const string Refunded = "Refunded";
    }
}