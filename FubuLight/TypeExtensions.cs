namespace FubuLight
{
    public static class TypeExtensions
    {
        public static T As<T>(this object input)
        {
            return (T) input;
        }
    }
}
