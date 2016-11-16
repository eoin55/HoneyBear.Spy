using System;
using System.Configuration;
using Autofac;
using HoneyBear.Spy.NLog.Autofac;
using HoneyBear.Spy.Sample.Library;
using HoneyBear.Spy.Serilog.Autofac;

namespace HoneyBear.Spy.Sample.Host.WindowsService
{
    internal static class IoCBootstrapper
    {
        public static IContainer Init()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<HostService>()
                .AsSelf();

            builder
                .RegisterType<Foo>()
                .AsSelf();

            LoggerType type;
            if (!Enum.TryParse(ConfigurationManager.AppSettings["HoneyBear.Spy.LoggerType"], true, out type))
                type = LoggerType.NLog;

            Console.WriteLine($"Using {nameof(LoggerType)}={type}");

            switch (type)
            {
                case LoggerType.Serilog:
                    builder.RegisterModule<SpySerilogAutofacModule>();
                    break;
                case LoggerType.NLog:
                    builder.RegisterModule<SpyNLogAutofacModule>();
                    break;
                default:
                    throw new NotSupportedException($"{type} {nameof(LoggerType)} not supported.");
            }
            
            return builder.Build();
        }
    }
}