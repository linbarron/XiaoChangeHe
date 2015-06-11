using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class CrmMemberScore
    {
       // public Guid Uid { get; set; }
        public int TotalScore { get; set; }
        public int LastScore { get; set; }
        public DateTime LastScoredDate { get; set; }
        public int Score { get; set; }
        public int UseScore { get; set; }
        public int UseMoney { get; set; }

    }
}