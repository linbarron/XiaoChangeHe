using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class Province
    {
        ///<summary>
        ///
        ///</summary>
        public string Id { get; set; }
        ///<summary>
        ///
        ///</summary>
        public string Name { get; set; }
        ///<summary>
        ///
        ///</summary>
        public string ParentId { get; set; }
        ///<summary>
        ///
        ///</summary>
        public int? SortNo { get; set; }
        ///<summary>
        ///
        ///</summary>
        public bool? IsUse { get; set; }

    }
}