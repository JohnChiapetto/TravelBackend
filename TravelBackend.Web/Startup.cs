using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using TravelBackend.Services;

[assembly: OwinStartup(typeof(TravelBackend.Web.Startup))]

namespace TravelBackend.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureAuth(app);

            var svc = new RoleService();
        }
    }
}
