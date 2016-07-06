using StructureMap;
using FubuLight.Registration;

namespace FubuLight
{
    public class FubuRegistry
    {
        private readonly Registry _registry;

        public FubuRegistry()
        {
            _registry = new DefaultRegistry();
        }

        public Registry Registry => _registry;

        public void IncludeRegistry<T>() where T:Registry, new()
        {
            _registry.IncludeRegistry(new T());
        }
    }
}