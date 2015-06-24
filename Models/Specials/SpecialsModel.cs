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
        /// 分页获取。
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public static List<SpecialsEntity> SelectAll(int startRowIndex, int maximumRows, string sortExpression)
        {
            using (var context = 
                new DbContext().ConnectionStringName("CrmRstV1", new SqlServerProvider()))
            {
                var select = context.Select<SpecialsEntity>(" * ")
                    .From(" Specials ");

                if (maximumRows > 0)
                {
                    if (startRowIndex == 0)
                        startRowIndex = 1;

                    select.Paging(startRowIndex, maximumRows);
                }

                if (!string.IsNullOrEmpty(sortExpression))
                    select.OrderBy(sortExpression);

                return select.QueryMany();
            }
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