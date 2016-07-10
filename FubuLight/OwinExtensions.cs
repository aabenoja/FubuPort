using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using FubuLight.Registration;

namespace FubuLight
{
    public static class OwinExtensions
    {
        public static Task WriteToBody(this IDictionary<string, object> environment, string body)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            var responseStream = environment["owin.ResponseBody"].As<Stream>();

            return responseStream.WriteAsync(bytes, 0, bytes.Length);
        }

        public static string RequestPath(this IDictionary<string, object> environment)
        {
            return environment["owin.RequestPath"].As<string>();
        }

        public static string RequestMethod(this IDictionary<string, object> environment)
        {
            return environment["owin.RequestMethod"].As<string>();
        }

        public static IContainer GetNestedContainer(this IDictionary<string, object> environment)
        {
            return environment["fubu.NestedContainer"].As<IContainer>();
        }

        public static void SetRouteData(this IDictionary<string, object> environment, Route route)
        {
            environment["fubu.RouteData"] = route;
        }

        public static Route GetRouteData(this IDictionary<string, object> environment)
        {
            return environment["fubu.RouteData"].As<Route>();
        }

        public static T GetActionResult<T>(this IDictionary<string, object> environment)
        {
            return environment["fubu.ActionResult"].As<T>();
        }
    }
}
