using System;
using NLog;
using Environment = HoneyBear.Spy.Configuration.Environment;

namespace HoneyBear.Spy.NLog.Configuration
{
    public class DefaultLogLevelDecider : ISpyLogLevelDecider
    {
        private const Environment DefaultEnvironment = Environment.Development;

        public LogLevel ByEnvironment(string environment)
        {
            Environment value;
            if (!Enum.TryParse(environment, true, out value))
                value = DefaultEnvironment;
            return ByEnvironment(value);
        }

        public LogLevel ByEnvironment(Environment environment)
        {
            switch (environment)
            {
                case Environment.Development:
                    return LogLevel.Debug;
                default:
                    return LogLevel.Info;
            }
        }
    }
}