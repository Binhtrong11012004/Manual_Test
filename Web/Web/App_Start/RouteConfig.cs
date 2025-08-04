using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //mắc định
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Web.Controllers" }
            );

            //mặc định admin
            routes.MapRoute(
                "Admin",
                "admin/{controller}/{action}/{id}",
                new { action = "index", id = UrlParameter.Optional },
                new[] { "Web.Areas.Admin.Controllers" }
            );
        }
    }
}
