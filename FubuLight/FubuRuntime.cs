using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using FubuLight.Registration;
using StructureMap;

namespace FubuLight
{
    public class FubuRuntime
    {
        private readonly FubuRegistry _registry;
        private readonly Container _container;
        private readonly IList<Route> _routes;

        public FubuRuntime(FubuRegistry registry)
        {
            _registry = registry;

            var callingAssembly = registry.GetType().GetTypeInfo().Assembly;
            var endpointClasses = findEndpoints(callingAssembly);

            _routes = RouteBuilder.BuildRoutes(endpointClasses);

            _container = new Container(registry.Registry);
        }

        public IList<Route> Routes => _routes;

        public Container Container => _container;

        private IEnumerable<Type> findEndpoints(Assembly assembly)
        {
            return assembly.DefinedTypes
                .Where(type => type.Name.EndsWith("Endpoint"))
                .Select(type => type.AsType())
                .ToList();
        }
    }
}