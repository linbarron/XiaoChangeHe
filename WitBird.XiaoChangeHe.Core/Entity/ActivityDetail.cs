using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Entity
{
    public class ActivityDetail
    {
        public string Title { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public string ContentText { get; set; }

        public string Link { get; set; }

        public string TimePlace
        {
            get
            {
                List<string> list = new List<string>();

                if (this.StartTime != null)
                {
                    list.Add(this.StartTime.Value.ToString("yyyy-MM-dd"));
                }

                if (!string.IsNullOrEmpty(this.Address))
                {
                    list.Add(this.Address);
                }

                return string.Join(" ", list);
            }
        }
    }
}
