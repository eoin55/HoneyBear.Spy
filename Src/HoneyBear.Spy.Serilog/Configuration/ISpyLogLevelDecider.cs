using HoneyBear.Spy.Configuration;
using Serilog.Events;

namespace HoneyBear.Spy.Serilog.Configuration
{
    public interface ISpyLogLevelDecider
    {
        LogEventLevel ByEnvironment(string environment);
        LogEventLevel ByEnvironment(Environment environment);
    }
}