using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace HoneyBear.Spy.Serilog.Enrichers
{
    internal class ClassAndMethodEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            var trace = new StackTrace();
            const int index = 5;
            if (trace.FrameCount < index)
            {
                logEvent.RemovePropertyIfPresent("ClassName");
                logEvent.RemovePropertyIfPresent("MethodName");
                return;
            }

            var method = trace.GetFrame(index).GetMethod();

            logEvent.AddOrUpdateProperty(factory.CreateProperty("ClassName", method.ReflectedType));
            logEvent.AddOrUpdateProperty(factory.CreateProperty("MethodName", method.ToString()));
        }
    }
}