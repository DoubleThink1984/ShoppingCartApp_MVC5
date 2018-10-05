using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ShoppingCartApp.Models;
using System.Web.WebPages;

namespace ShoppingCartApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //wildman1Entities1 sp = new wildman1Entities1();
        protected void Application_Start()
        {
            var displayModes = DisplayModeProvider.Instance.Modes;

            Database.SetInitializer<ShoppingCartApp.Models.ApplicationDbContext>(null);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            new InitializeAdmin();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var raisedException = Server.GetLastError();

            Server.ClearError();

            Response.Redirect("~/Home/Error");
            // Process exception
        }
    }
}
