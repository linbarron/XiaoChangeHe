using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Restaurant
{
    [Serializable]
    public class RestaurantImage
    {
        public Int32 ImageId { get; set; }
        public string Url { get; set; }
        public Guid? RestaurantId { get; set; }
        public int OrderBy { get; set; }
        public int State { get; set; }
        public DateTime? CreatedTime { set; get; }
    }
}