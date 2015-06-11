using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class ProductType
    {
        ///<summary>
        ///商品类型编码
        ///</summary>
        public Guid TypeId { get; set; }
        ///<summary>
        ///商品类型名称
        ///</summary>
        public string TypeName { get; set; }
        ///<summary>
        ///排序编号
        ///</summary>
        public Int32? OrderNo { set; get; }
        ///<summary>
        ///父级商品类型编码
        ///</summary>
        public Guid ParentType { get; set; }
        ///<summary>
        ///打印机编号
        ///</summary>
        public int? PrintId { get; set; }
        /// <summary>
        /// 是否服务类型
        /// </summary>
        public bool? IsServiceType { set; get; }
        /// <summary>
        /// 所属店面
        /// </summary>
        public Guid RestaurantId { set; get; }
    }
}