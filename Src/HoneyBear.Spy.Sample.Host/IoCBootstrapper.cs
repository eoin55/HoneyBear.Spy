using Autofac;
using HoneyBear.Spy.Sample.Library;
using HoneyBear.Spy.Serilog.Autofac;
using HoneyBear.Spy.Serilog.Configuration;

namespace HoneyBear.Spy.Sample.Host
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

            builder.RegisterModule<SpySerilogAutofacModule<DefaultLogLevelDecider>>();

            return builder.Build();
        }
    }
}