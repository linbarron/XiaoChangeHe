using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe.Models.Info
{
    public class RandomProduct
    {

        public Guid Id { get; set; }
        //public Guid TypeId { get; set; }
        public string ProductName { get; set; }
      //  public Guid RestaurantId { get; set; }
        //public bool Status { get; set; }
        public decimal Price { get; set; }
        public int Popular { get; set; }
        public int Hot { get; set; }
        public string ADescription { get; set; }
    }

    public class AutoProducts
    {

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int ProductCount { get; set; }
        public int LoveCount { get; set; }
        public int Count { get; set; }
        public int Hot { get; set; }
        public string Description { get; set; }
        public Boolean Popular { get; set; }
       
    }
}