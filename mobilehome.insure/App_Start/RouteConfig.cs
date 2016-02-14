using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MobileHome.Insure.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Unsubscribe",
                url: "Unsubscribe",
                 defaults: new { controller = "Home", action = "Unsubscribe", id = UrlParameter.Optional },
                 namespaces: new[] { "MobileHome.Insure.Web.Controllers" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MobileHome.Insure.Web.Controllers" }
            );

        }
    }
}