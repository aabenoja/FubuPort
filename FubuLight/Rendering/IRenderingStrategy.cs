using System.Collections.Generic;

namespace FubuLight.Rendering
{
    public interface IRenderingStrategy
    {
        string ToResponseBody(object response);
        bool Matches(IDictionary<string, object> environment, object response);
    }

    public class DefaultRenderingStrategy : IRenderingStrategy
    {
        public bool Matches(IDictionary<string, object> environment, object response)
        {
            return false;
        }

        public string ToResponseBody(object response)
        {
            return response.ToString();
        }
    }
}
