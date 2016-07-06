using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FubuLight.Registration;
using FubuLight.Rendering;
using StructureMap;
using System;

namespace FubuLight.Hosting.Owin
{
    public class FubuOwinHost
    {
        private readonly IList<Route> _routes;
        private readonly IContainer _container;

        public FubuOwinHost(IList<Route> routes, IContainer container)
        {
            _routes = routes;
            _container = container;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            using (var nestedContainer = _container.GetNestedContainer())
            {
                var renderer = nestedContainer.GetInstance<IRenderer>();

                var route = _routes.FirstOrDefault(routeInfo => routeInfo.Condition(environment));
                var message = route != null ? "Route found!" : "Route not found!";
                var handler = nestedContainer.GetInstance(route.HandlerType);

                // create input object
                var output = route.Method.Invoke(handler, new object[] {});
                var response = renderer.ToResponseBody(environment, output);

                return environment.WriteToBody(output.As<string>());
            }
        }

        private Task defaultHandler(IDictionary<string, object> environment)
        {
            var path = environment.RequestPath();
            var response = $"{path} works!";
            return environment.WriteToBody(response);
        }
    }
}