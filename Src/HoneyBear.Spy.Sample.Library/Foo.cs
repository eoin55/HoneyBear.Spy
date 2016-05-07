namespace HoneyBear.Spy.Sample.Library
{
    public class Foo
    {
        private readonly ISpy _spy;

        public Foo(ISpy spy)
        {
            _spy = spy;
        }

        public void Bar()
        {
            _spy.Debug("I'm a debug message from {0}", _spy.GetType());
            _spy.Info("Hello, world!");
        }
    }
}