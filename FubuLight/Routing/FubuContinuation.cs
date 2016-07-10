namespace FubuLight.Routing
{
    public class FubuContinuation
    {
        private readonly ContinuationType _type;

        public FubuContinuation(ContinuationType type)
        {
            _type = type;
        }

        public ContinuationType Type => _type;
    }
}
