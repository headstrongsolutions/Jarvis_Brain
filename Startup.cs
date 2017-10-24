using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Owin;

namespace Jarvis_Brain
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "WithActionApi",
                "api/{controller}/{action}/{id}"
            );

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { action="DefaultAction", id = System.Web.Http.RouteParameter.Optional }
            );
            
            config.EnableCors();

            config.EnsureInitialized();

            appBuilder.UseWebApi(config);

        }
    } 
}
