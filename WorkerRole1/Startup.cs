using Owin;
using System.Web.Http;

namespace WorkerRole1
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "Default",
                "{controller}/{name}",
               new { name = RouteParameter.Optional });

            app.UseWebApi(config);
        }
    }
}
