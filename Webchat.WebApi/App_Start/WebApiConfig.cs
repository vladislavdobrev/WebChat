using System;
using System.Linq;
using System.Web.Http;

namespace Webchat.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors();

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
