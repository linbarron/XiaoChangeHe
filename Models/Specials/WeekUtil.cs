using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WitBird.XiaoChangHe.Models.Specials
{
    public static class WeekUtil
    {
        public static string GetCN(this HtmlHelper helper)
        {
            var dayOfWeek = string.Empty;

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayOfWeek = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeek = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeek = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeek = "星期四";
                    break;
                case DayOfWeek.Friday:
                    dayOfWeek = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeek = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    dayOfWeek = "星期日";
                    break;
            }

            return dayOfWeek;
        }
    }
}