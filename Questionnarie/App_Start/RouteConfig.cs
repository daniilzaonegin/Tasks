using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tasks
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null,
                "",
                new {
                    controller = "Home", action = "TaskList",
                    selection = (string)null, page = 1
                });

            routes.MapRoute(
                null,
                "Page{page}",
                new { controller = "Home", action = "TaskList", selection = (string)null},
                new { page=@"\d+" });

            routes.MapRoute(null, 
                "{selection}", 
                new { controller = "Home", action = "TaskList", page = 1 }
                );

            routes.MapRoute(null,
                "{selection}/Page{page}",
                new { controller = "Home", action = "TaskList"},
                new {page=@"\d+"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "TaskList", id = UrlParameter.Optional }
            );
        }
    }
}
