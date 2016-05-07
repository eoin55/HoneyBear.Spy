namespace HoneyBear.Spy.Configuration
{
    public interface ISpyLogLevel
    {
        ISpyBuilder Value(string level);
        ISpyBuilder OverriddenFromAppSetting();
        ISpyBuilder ByEnvironment(string environment);
        ISpyBuilder ByEnvironment(Environment environment);
        ISpyBuilder ByEnvironmentFromAppSetting();
    }
}