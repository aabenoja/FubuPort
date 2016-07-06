namespace FubuLight.Registration
{
    public class RoutePathInfo
    {
        private string _method;

        public string Method
        {
            get { return _method; }
            set { _method = value.ToUpper(); }
        }

        public string Path { get; set; }

        public static RoutePathInfo From(string methodName)
        {
            var methodPieces = methodName.Split('_');
            var requestMethod = methodPieces[0];

            return new RoutePathInfo
            {
                Method = requestMethod
            };
        }
    }
}
