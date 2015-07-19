using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitBird.XiaoChangeHe.Core.Entity
{
    public class StatResult
    {
        public GuestPair GuestPair { get; set; }

        public List<Dish> DishList { get; set; }
    }

    public class GuestPair
    {
        public string GuestName1 { get; set; }

        public string GuestName2 { get; set; }

        public int Count { get; set; }
    }

    public class Dish
    {
        public string Name { get; set; }

        public int Count { get; set; }
    }
}
