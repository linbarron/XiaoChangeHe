using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WitBird.XiaoChangeHe.Core.Dal;
using WitBird.XiaoChangeHe.Core.Entity;

namespace WitBird.XiaoChangeHe.Core
{
    public class StatManager
    {
        public StatResult GetStatResult()
        {
            StatResult result = new StatResult();

            StatDal statDal = new StatDal();
            MediaGuestDal mediaGuestDal = new MediaGuestDal();

            var guestDishes = statDal.GetGuestDishes();

            if (guestDishes != null && guestDishes.Count > 0)
            {
                #region 最有缘的嘉宾
                List<GuestPair> pairs = new List<GuestPair>();
                var guests = guestDishes.GroupBy(v => v.GuestId).ToList();
                for (int i = 0; i < guests.Count; i++)
                {
                    for (int j = i + 1; j < guests.Count; j++)
                    {
                        var count = (from t in (from a in guests[i] join b in guests[j] on a.ProductId equals b.ProductId select new { pid = a.ProductId }) group t by t.pid).Count();

                        pairs.Add(new GuestPair { GuestId1 = guests[i].Key, GuestId2 = guests[j].Key, Count = count });
                    }
                }
                if (pairs != null && pairs.Count > 0)
                {
                    var topPair = pairs.OrderByDescending(v => v.Count).First();

                    var mediaGuests = mediaGuestDal.GetGuests();
                    if (mediaGuests != null && mediaGuests.Count > 0)
                    {
                        result.GuestPair = new Entity.GuestPair();
                        result.GuestPair.GuestName1 = mediaGuests.First(v => v.Id == topPair.GuestId1).RealName;
                        result.GuestPair.GuestName2 = mediaGuests.First(v => v.Id == topPair.GuestId2).RealName;
                        result.GuestPair.Count = topPair.Count;
                    }
                }
                #endregion

                #region 最受欢迎的十大菜品
                var populars = (from dishGroup in (from dish in guestDishes group dish by dish.ProductId) orderby dishGroup.Count() descending select new { Key = dishGroup.Key, Count = dishGroup.Count() }).Take(10).ToList();
                ProductDal productDal = new ProductDal();
                var products = productDal.GetProductsById(populars.Select(v => v.Key).ToList());
                if (products != null && products.Count > 0)
                {
                    result.DishList = new List<Dish>();
                    foreach (var item in populars)
                    {
                        var product = products.FirstOrDefault(v => v.ProductId == item.Key);
                        if (product != null)
                        {
                            var dish = new Dish();

                            dish.Name = product.ProductName;
                            dish.Count = item.Count;

                            result.DishList.Add(dish);
                        }
                    }
                }

                #endregion
            }

            return result;
        }

        private class GuestPair
        {
            public int GuestId1 { get; set; }

            public int GuestId2 { get; set; }

            public int Count { get; set; }
        }
    }
}
