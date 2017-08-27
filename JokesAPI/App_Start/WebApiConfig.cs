using System.Web.Http;
using System.Web.Http.Cors;

namespace ContactsList.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Uncomment the following line to control CORS by using Web API code

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
