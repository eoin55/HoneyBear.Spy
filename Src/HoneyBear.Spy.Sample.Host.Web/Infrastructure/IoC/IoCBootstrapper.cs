using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using HoneyBear.Spy.NLog.Autofac;
using HoneyBear.Spy.Sample.Library;
using HoneyBear.Spy.Serilog.Autofac;

namespace HoneyBear.Spy.Sample.Host.Web.Infrastructure.IoC
{
    internal static class IoCBootstrapper
    {
        public static IContainer Init()
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterApiControllers(assembly);
            builder.RegisterBootstrappers(assembly);
            builder.RegisterModules();

            builder
                .RegisterType<Foo>()
                .AsSelf();

            var container = builder.Build();

            container.ConfigureDependencyResolver();
            container.InitBootstrappers();

            return container;
        }

        private static void RegisterBootstrappers(this ContainerBuilder builder, Assembly assembly) =>
            builder
                .RegisterAssemblyTypes(assembly)
                .Where(t => typeof(IBootstrapper).IsAssignableFrom(t))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

        private static void RegisterModules(this ContainerBuilder builder)
        {
            builder.RegisterModule<WebApiAutofacModule>();

            LoggerType type;
            if (!Enum.TryParse(ConfigurationManager.AppSettings["HoneyBear.Spy.LoggerType"], true, out type))
                type = LoggerType.NLog;

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
        }

        private static void ConfigureDependencyResolver(this ILifetimeScope container) =>
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        private static void InitBootstrappers(this IComponentContext container)
        {
            foreach (var bootstrapper in container.Resolve<IEnumerable<IBootstrapper>>())
                bootstrapper.Init();
        }
    }
}