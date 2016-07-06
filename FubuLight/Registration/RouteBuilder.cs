using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FubuLight.Registration
{
    public static class RouteBuilder
    {
        public static IList<Route> BuildRoutes(IEnumerable<Type> types)
        {
            var routeCollection = new List<Route>();

            foreach (var type in types)
            {
                routeCollection.AddRange(type.CreateRoutes());
            }

            return routeCollection;
        }

        public static IEnumerable<Route> CreateRoutes(this Type type)
        {
            var isHome = type.Name.StartsWith("Home");

            return type.GetMethods()
                .Where(method => method.DeclaringType != typeof(object))
                .Select(method => method.CreateRoute(type, isHome));
        }

        public static Route CreateRoute(this MethodInfo method, Type type, bool isHome)
        {
            Console.WriteLine($"Building route for {method.Name}");
            if (isHome && method.Name == "get_Index")
            {
                return new Route(type, method, environment => environment.RequestPath() == "/");
            }

            var pathInfo = RoutePathInfo.From(method.Name);

            return new Route(type, method, environment =>
                environment.RequestMethod() == pathInfo.Method
                    && environment.RequestPath() == pathInfo.Path
            );
        }
    }
}
