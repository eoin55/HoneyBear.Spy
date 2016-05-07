using HoneyBear.Spy.Sample.Library;

namespace HoneyBear.Spy.Sample.Host
{
    internal class HostService
    {
        private readonly ISpy _spy;
        private readonly Foo _foo;

        public HostService(
            ISpy spy,
            Foo foo)
        {
            _spy = spy;
            _foo = foo;
        }

        public void Start()
        {
            _spy.Info("Starting...");

            _foo.Bar();
        }

        public void Stop()
        {
            _spy.Info("Stopping...");
        }
    }
}