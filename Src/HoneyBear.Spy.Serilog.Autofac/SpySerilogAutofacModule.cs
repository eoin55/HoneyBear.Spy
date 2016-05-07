using Autofac;
using HoneyBear.Spy.Configuration;
using HoneyBear.Spy.Serilog.Configuration;

namespace HoneyBear.Spy.Serilog.Autofac
{
    public class SpySerilogAutofacModule : SpySerilogAutofacModule<DefaultLogLevelDecider>
    {

    }

    public class SpySerilogAutofacModule<T> : Module where T : ISpyLogLevelDecider
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<T>()
                .AsImplementedInterfaces();

            builder
                .RegisterType<SpyBuilder>()
                .AsImplementedInterfaces();

            builder
                .Register(
                    context =>
                        context
                            .Resolve<ISpyBuilder>()
                                .WithLogLevel
                                    .ByEnvironmentFromAppSetting()
                                .WithLogLevel
                                    .OverriddenFromAppSetting()
                                .WithFileLocation
                                    .OverriddenFromEnvironmentVariable()
                                .WithFileLocation
                                    .OverriddenFromAppSetting()
                                .WithFileLocation
                                    .AppendProductNameFromAppSetting()
                                .WithFileName
                                    .OverriddenFromAppSetting()
                            .Create())
                .As<ISpy>()
                .SingleInstance();
        }
    }
}