using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web;
using AuthPackage;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Code that runs on application startup

        }
    }
}