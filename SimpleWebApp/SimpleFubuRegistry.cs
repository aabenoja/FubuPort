using FubuLight;
using StructureMap;

namespace SimpleWebApp
{
    public class SimpleFubuRegistry : FubuRegistry
    {
        public SimpleFubuRegistry()
        {
            IncludeRegistry<SimpleRegistry>();
        }
    }

    public class SimpleRegistry : Registry
    {
        public SimpleRegistry()
        {
            Scan(_ =>
            {
                _.AssemblyContainingType<IHelloMessage>();
                _.WithDefaultConventions();
            });
        }
    }

    public interface IHelloMessage
    {
        string Message();
    }

    public class HelloMessage : IHelloMessage
    {
        public string Message()
        {
            return "This is a hello message";
        }
    }
}
