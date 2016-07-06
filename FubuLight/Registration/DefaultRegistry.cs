using StructureMap;
using FubuLight.Rendering;

namespace FubuLight.Registration
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(_ =>
            {
                _.AssemblyContainingType<IRenderingStrategy>();
                _.AddAllTypesOf<IRenderingStrategy>();
            });

            For<IRenderer>().Use<Renderer>().Named("Default Renderer");
        }
    }
}
