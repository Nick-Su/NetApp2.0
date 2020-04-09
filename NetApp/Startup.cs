using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(NetApp.Startup))]
namespace NetApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed i user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Index")
            });

            // Any connection or hub wire up and configuration should go here
            AreaRegistration.RegisterAllAreas();
            ConfigureAuth(app);
            app.MapSignalR();
            
        }
    }
}
