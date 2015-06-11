using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WitBird.XiaoChangHe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Default2", // 路由名称
               url: "{controller}/{action}/{id}/{name}", // 带有参数的 URL
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, name = UrlParameter.Optional } // 参数默认值
                //new { controller = "Edit", action = "Index", id = UrlParameter.Optional } // 参数默认值
           );
        }
    }
}