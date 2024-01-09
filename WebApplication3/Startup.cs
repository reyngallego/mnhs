using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(WebApplication3.Startup))]

namespace WebApplication3
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            ConfigureSignalR(app);
        }

        private void ConfigureSignalR(IAppBuilder app)
        {
            // Configure SignalR here
            app.MapSignalR();
            ConfigureApi(app);

        }
    }
}