using NLog;
using Environment = HoneyBear.Spy.Configuration.Environment;

namespace HoneyBear.Spy.NLog.Configuration
{
    public interface ISpyLogLevelDecider
    {
        LogLevel ByEnvironment(string environment);
        LogLevel ByEnvironment(Environment environment);
    }
}