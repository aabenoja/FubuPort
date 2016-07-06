using System.Collections.Generic;
using System.Linq;

namespace FubuLight.Rendering
{
    public interface IRenderer
    {
        string ToResponseBody(IDictionary<string, object> environment, object response);
    }

    public class Renderer : IRenderer
    {
        private readonly IEnumerable<IRenderingStrategy> _strategies;
        private readonly DefaultRenderingStrategy _defaultStrategy;

        public Renderer(IEnumerable<IRenderingStrategy> strategies, DefaultRenderingStrategy defaultStrategy)
        {
            _strategies = strategies;
            _defaultStrategy = defaultStrategy;
        }

        public string ToResponseBody(IDictionary<string, object> environment, object response)
        {
            var strategy = _strategies.FirstOrDefault(_ => _.Matches(environment, response)) ?? _defaultStrategy;
            return strategy.ToResponseBody(response);
        }
    }
}
