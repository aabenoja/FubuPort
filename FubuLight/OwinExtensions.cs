using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
    }
}
