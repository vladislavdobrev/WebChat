using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Webchat.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors();
            // Other configuration omitted
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            config.Routes.MapHttpRoute(
                   name: "UsersGetApi",
                   routeTemplate: "api/users/get/{id}",
                   defaults: new { controller = "users" }
               );

            config.Routes.MapHttpRoute(
                   name: "UsersApi",
                   routeTemplate: "api/users/{action}/{sessionKey}",
                   defaults: new { controller = "users" }
               );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{sessionKey}",
                defaults: new { sessionKey = RouteParameter.Optional, id = RouteParameter.Optional }
            );
        }
    }
}
