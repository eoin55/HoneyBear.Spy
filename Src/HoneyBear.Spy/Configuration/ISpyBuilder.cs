namespace HoneyBear.Spy.Configuration
{
    public interface ISpyBuilder
    {
        ISpyLogLevel WithLogLevel { get; }
        ISpyFileLocation WithFileLocation { get; }
        ISpyFileName WithFileName { get; }
        ISpy Create();
    }
}