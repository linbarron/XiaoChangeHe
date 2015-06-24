using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Models.Specials
{
    [Serializable]
    public class SpecialsEntity
    {
        public Int32 Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? RestaurantId { get; set; }
        public Decimal? SpecPrice { get; set; }
        public Int32? WeekDateUse { get; set; }
        public Boolean? Stauts { get; set; }
        public DateTime? UseDate { get; set; }

        public ProductNew Product { set; get; }
    }
}