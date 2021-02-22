using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RecipeForSuccess_mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute("ChangePassword", "Account/ChangePassword", new { controller = "Account", action = "ChangePassword" });

            routes.MapRoute("Profile", "Profile/{action}/{id}", new { controller = "Profile", action = "Index", id=UrlParameter.Optional });

            routes.MapRoute("Login", "Account/Login", new { controller = "Account", action = "Login" });
            routes.MapRoute("Logout", "Account/Logout", new { controller = "Account", action = "Logout" });

            routes.MapRoute("Account", "{username}", new { controller = "Account", action = "Username" });
            routes.MapRoute("CreateAccount", "Account/Register", new { controller = "Account", action = "Register" });

            //routes.MapRoute("Default", "Home/Index", new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );
        }
    }
}
