using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class ReceiveOrder
    {

        public Guid ID { get; set; }

        public Guid RstId { get; set; }

        public string ReceiveTime { get; set; }

        public string TableCount { get; set; }

        public int? MaxTime { get; set; }
    }

    public class ReceiveOrder1
    {
       // public Guid ID { get; set; }

       // public Guid RstId { get; set; }

        public byte[] DefaultImg { get; set; }
    }

    public class ReceiveOrder2
    {

       // public Guid ID { get; set; }

       // public Guid RstId { get; set; }

      //  public string ReceiveTime { get; set; }

       // public string TableCount { get; set; }

       // public int? MaxTime { get; set; }

        public string Explain { get; set; }
    }

}