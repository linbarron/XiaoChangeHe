using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentData;

namespace WitBird.XiaoChangHe.Models.Specials
{
    public class SpecialsModel
    {
        /// <summary>
        /// 获取今天所有特价菜。
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public static List<SpecialsEntity> GetTodayByRestaurantId(Guid restaurantId)
        {
            List<SpecialsEntity> result = null;
            try
            {
                using (var context =
                    new DbContext().ConnectionStringName("CrmRstV1", new SqlServerProvider()))
                {
                    int dayOfWeek = (int) DateTime.Now.DayOfWeek;
                    if (dayOfWeek == 0)
                    {
                        dayOfWeek = 7;
                    }
                    var select = context.StoredProcedure("sp_GetTodayByRestaurantId")
                        .Parameter("RestaurantId", restaurantId)
                        .Parameter("WeekDateUse", dayOfWeek);
                    result =  select.QueryMany<SpecialsEntity>();
                }
            }
            catch (Exception)
            {
                //TODO
            }
            if (result == null)
            {
                result=new List<SpecialsEntity>();
            }
            return result;
        }

        public static List<SpecialsEntity> GetAllByRestaurantId(Guid restaurantId)
        {
            List<SpecialsEntity> result = null;
            try
            {
                using (var context =
                    new DbContext().ConnectionStringName("CrmRstV1", new SqlServerProvider()))
                {
                    int dayOfWeek = (int)DateTime.Now.DayOfWeek;
                    if (dayOfWeek == 0)
                    {
                        dayOfWeek = 7;
                    }
                    var select = context.StoredProcedure("sp_GetAllByRestaurantId")
                        .Parameter("RestaurantId", restaurantId)
                        .Parameter("WeekDateUse", dayOfWeek);
                    result = select.QueryMany<SpecialsEntity>();
                }
            }
            catch (Exception)
            {
                //TODO
            }
            if (result == null)
            {
                result = new List<SpecialsEntity>();
            }
            return result;
        }

        /// <summary>
        /// 根据餐馆Id 得到特价菜数量。
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public static int CountAll(Guid restaurantId)
        {
            using (var context = new DbContext().ConnectionStringName("CrmRstV1", new SqlServerProvider()))
            {
                return context.Sql(" SELECT COUNT(*) FROM Specials ")
                    .QuerySingle<int>();
            }
        }
    }
}