namespace SimpleWebApp
{
    public class HomeEndpoint
    {
        private readonly IHelloMessage _message;

        public HomeEndpoint(IHelloMessage message)
        {
            _message = message;
        }

        public string get_Index()
        {
            return _message.Message();
        }
    }
}
