using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class Product
    {
        public Guid Id { get; set; }
        //public Guid TypeId { get; set; }
        public string ProductName { get; set; }
        public Guid ProductType { get; set; }
        public string Code { get; set; }
        public string PinYin { get; set; }
        public string Unit { get; set; }
        public string MinUnit { get; set; }
        public int MinCount { get; set; }
        public decimal Price { get; set; }
        public byte[] OriginalImage { get; set; }
        public byte[] ThumbImage { get; set; }
        public string BarCode { get; set; }
        public bool Status { get; set; }
        public Guid RestaurantId { get; set; }
        public bool? Type { get; set; }
        public Guid MaterialId { get; set; }
        public bool? IsDiscount { get; set; }
        public string ProductTypeName1 { get; set; }
        public string ProductTypeName2 { get; set; }
        public int OrderNo { get; set; }
        public bool IsServiceType { get; set; }

        public int OrderCount { get; set; }
        public int FavCount { get; set; }

    }

    public class MemProduct {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public Decimal? MemberPrice { get; set; }
        public Decimal? Price { get; set; }
        public string CodeTypeListName { get; set; }
    
    }

    public class ProductMenu {
        public byte[] ThumbImage { get; set; }
    }

    public class ProductNew
    {
        ///<summary>
        ///商品编号
        ///</summary>
        public Guid Id { get; set; }
        ///<summary>
        ///商品名称
        ///</summary>
        public string ProductName { get; set; }
        ///<summary>
        ///商品类型编号
        ///</summary>
        public Guid ProductType { get; set; }
        ///<summary>
        ///商品编码
        ///</summary>
        public string Code { get; set; }
        ///<summary>
        ///拼音助记码
        ///</summary>
        public string PinYin { get; set; }
        ///<summary>
        ///单位编号
        ///</summary>
        public string Unit { get; set; }
        ///<summary>
        ///最小单位
        ///</summary>
        public string MinUnit { get; set; }
        ///<summary>
        ///最小单位数量
        ///</summary>
        public int? MinCount { get; set; }
        ///<summary>
        ///单价
        ///</summary>
        public Decimal? Price { get; set; }
        ///使用状态
        ///</summary>
        public bool? Status { get; set; }
        ///<summary>
        ///店面编号
        ///</summary>
        public Guid RestaurantId { get; set; }
        ///<summary>
        ///是否去消耗表扣除
        ///</summary>
        public bool? Type { get; set; }
        ///<summary>
        ///物料编号
        ///</summary>
        public Guid MaterialId { get; set; }
        ///<summary>
        ///是否可以打折
        ///</summary>
        public bool? IsDiscount { get; set; }


        ///<summary>
        ///会员单价
        ///</summary>
        public Decimal? MemberPrice { get; set; }
      //  public int OrderCount { get; set; }
       // public int FavCount { get; set; }
        public string CodeTypeListName { get; set; }
        public int Count{get;set;}
        public int LoveCount{get;set;}
        //辣度
        public int Hot { get; set; }
        public string Description { get; set; }
        //是否是好评菜
        public Boolean Popular { get; set; }
    }

   
}
