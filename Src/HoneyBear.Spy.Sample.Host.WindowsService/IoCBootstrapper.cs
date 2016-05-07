using Autofac;
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

            builder.RegisterModule<SpySerilogAutofacModule>();

            return builder.Build();
        }
    }
}