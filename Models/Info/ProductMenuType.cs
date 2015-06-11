using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class ProductMenuType
    {
        ///<summary>
///类型编号
///</summary>
public Guid TypeId{ get; set; }
///<summary>
///类型名称
///</summary>
public string TypeName{ get; set; }
///<summary>
///类型排序
///</summary>
public int OrderNo{ get; set; }
///<summary>
///父级类型
///</summary>
public Guid ParentType{ get; set; }
///<summary>
///打印机编号
///</summary>
public int? PrintId{ get; set; }
///<summary>
///
///</summary>
public bool IsServiceType{ get; set; }
///<summary>
///公司编号
///</summary>
public Guid CompanyId{ get; set; }
}

    
}