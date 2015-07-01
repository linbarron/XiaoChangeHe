using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Dal.Entity
{
    public class ActivityDetail
    {
        public string Address { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
    }
}
