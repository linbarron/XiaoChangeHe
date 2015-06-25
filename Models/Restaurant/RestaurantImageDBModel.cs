using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentData;
using WitBird.XiaoChangHe.Models.Specials;

namespace WitBird.XiaoChangHe.Models.Restaurant
{
    public class RestaurantImageDBModel
    {
        public static List<RestaurantImage> GetRestaurantImages(Guid restaurantId)
        {
            List<RestaurantImage> result = null;
            try
            {
                using (var context =
                    new DbContext().ConnectionStringName("CrmRstV1", new SqlServerProvider()))
                {
                    var select = context.StoredProcedure("SP_GetRestaurantImages")
                        .Parameter("RestaurantId", restaurantId);
                    result = select.QueryMany<RestaurantImage>();
                }
            }
            catch (Exception)
            {
                //TODO
            }
            if (result == null)
            {
                result = new List<RestaurantImage>();
            }
            return result;
        }
    }
}