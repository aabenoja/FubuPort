using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace FubuLight.Hosting.Owin
{
  public delegate Task AppFunc(IDictionary<string, object> environment);

  public static class OwinMiddleware
  {
    public static void UseFubuRegistry<T>(this IApplicationBuilder app) where T:FubuRegistry, new()
    {
      var registry = new T();
      var runtime = new FubuRuntime(registry);
      var host = new FubuOwinHost(runtime.Routes, runtime.Container);

      app.UseOwin(host.ApplyMiddleware);
    }
  }
}