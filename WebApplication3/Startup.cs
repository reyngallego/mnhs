using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Formatting;

[assembly: OwinStartup(typeof(WebApplication3.Startup))]

namespace WebApplication3
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            // Enable CORS
            app.UseCors(CorsOptions.AllowAll);

            // Configure Web API
            ConfigureWebApi(app);

            // Configure SignalR
            app.MapSignalR();
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Enable attribute-based routing
            config.MapHttpAttributeRoutes();

            // Enable CORS for Web API
            var corsAttribute = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttribute);

            // Set formatters if necessary
            config.Formatters.Add(new System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter());
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

            // Configure other Web API settings...

            // Use Web API configuration
            app.UseWebApi(config);
        }

    }
}
