using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class Extension
    {
        ///<summary>
        ///编号
        ///</summary>
        public Guid EId { get; set; }
        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }
        ///<summary>
        ///描述
        ///</summary>
        public string Econtent { get; set; }
        ///<summary>
        ///<summary>
        ///日期
        ///</summary>
        public DateTime Datetime { get; set; }
        ///<summary>
        ///编辑人员
        ///</summary>
        public string EditMan { get; set; }
        ///<summary>
        ///公司编号
        ///</summary>
        public Guid CompanyId { get; set; }
        ///<summary>
        ///总编辑序号
        ///</summary>
        public string Etype { get; set; }
        ///<summary>
        ///序号
        ///</summary>
        public int? OrderNo { get; set; }
        public string Name { get; set; }
    }






    public class ExtensionImg
    {
        ///<summary>
        ///编号
        ///</summary>
        public Guid EId { get; set; }
        ///<summary>

        ///图片
        ///</summary>
        public Byte[] Photo { get; set; }
        ///<summary>
    }
}