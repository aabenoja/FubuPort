using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FubuLight.Registration;
using FubuLight.Rendering;
using StructureMap;
using System;

namespace FubuLight.Hosting.Owin
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using CreateMiddleware = Func<AppFunc, AppFunc>;
    using AddMiddleware = Action<Func<
        Func<IDictionary<string, object>, Task>,
        Func<IDictionary<string, object>, Task>
    >>;

    public class FubuOwinHost
    {
        private readonly IList<Route> _routes;
        private readonly IContainer _container;

        public const string ROUTE_DATA = "fubu.RouteData";
        public const string ACTION_RESULT = "fubu.ActionResult";
        public const string RESPONSE_BODY = "fubu.ResponseBody";

        public FubuOwinHost(IList<Route> routes, IContainer container)
        {
            _routes = routes;
            _container = container;
        }

        public void ApplyMiddleware(AddMiddleware pipeline)
        {
            pipeline(SetNestedContainer);
            pipeline(RouteDataLookup);
            pipeline(WriteResponse);
            pipeline(GenerateResponseBody);
            // handle transfers
            // handle redirect responses

            // chance to set other middleware, such as authentication/authorization

            // pipeline(HandleTransfer);
            pipeline(ExecuteRouteHandler);

            // post-route middleware
        }

        public Func<IDictionary<string, object>, Task> HandleTransfer(Func<IDictionary<string, object>, Task> next)
        {
            return async environment =>
            {
                if (!environment.ContainsKey("fubu.ActionResult"))
                {
                    await next(environment);
                }

                // if action result is null
                //    await next(env);
                // if action result is a transfer request
                //    apply transfer data
                while (true)
                {
                    await next(environment);
                    //check if action result is 
                }
            };
        }

        public Func<IDictionary<string, object>, Task> SetNestedContainer(Func<IDictionary<string, object>, Task> next)
        {
            return async environment =>
            {
                using (var nestedContainer = _container.GetNestedContainer())
                {
                    var nestedContainerKey = "fubu.NestedContainer";
                    environment[nestedContainerKey] = nestedContainer;
                    await next(environment).ConfigureAwait(false);
                    environment.Remove(nestedContainerKey); //probably don't need to do this?
                }
            };
        }

        public Func<IDictionary<string, object>, Task> RouteDataLookup(Func<IDictionary<string, object>, Task> next)
        {
            return environment =>
            {
                var route = _routes.FirstOrDefault(routeInfo => routeInfo.Condition(environment));
                environment[ROUTE_DATA] = route;
                return next(environment);
            };
        }

        public Func<IDictionary<string, object>, Task> ExecuteRouteHandler(Func<IDictionary<string, object>, Task> next)
        {
            return environment =>
            {
                var nestedContainer = environment.GetNestedContainer();
                var route = environment.GetRouteData();
                var handler = nestedContainer.GetInstance(route.HandlerType);

                // create input object
                var output = route.Method.Invoke(handler, new object[] {});
                environment[ACTION_RESULT] = output;

                return next(environment);
            };
        }

        public Func<IDictionary<string, object>, Task> GenerateResponseBody(Func<IDictionary<string, object>, Task> next)
        {
            return async environment =>
            {
                await next(environment);
                var nestedContainer = environment.GetNestedContainer();
                var output = environment[ACTION_RESULT];
                var renderer = nestedContainer.GetInstance<IRenderer>();

                var response = renderer.ToResponseBody(environment, output);
                environment[RESPONSE_BODY] = response;
            };
        }

        public Func<IDictionary<string, object>, Task> WriteResponse(Func<IDictionary<string, object>, Task> next)
        {
            return async environment =>
            {
                await next(environment);

                var output = environment[RESPONSE_BODY].As<string>();
                await environment.WriteToBody(output);
            };
        }
    }
}