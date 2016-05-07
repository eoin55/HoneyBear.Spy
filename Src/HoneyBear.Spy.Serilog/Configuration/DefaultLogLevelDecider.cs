using System;
using Serilog.Events;
using Environment = HoneyBear.Spy.Configuration.Environment;

namespace HoneyBear.Spy.Serilog.Configuration
{
    public class DefaultLogLevelDecider : ISpyLogLevelDecider
    {
        private const Environment DefaultEnvironment = Environment.Development;

        public LogEventLevel ByEnvironment(string environment)
        {
            Environment value;
            if (!Enum.TryParse(environment, true, out value))
                value = DefaultEnvironment;
            return ByEnvironment(value);
        }

        public LogEventLevel ByEnvironment(Environment environment)
        {
            switch (environment)
            {
                case Environment.Development:
                    return LogEventLevel.Debug;
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}