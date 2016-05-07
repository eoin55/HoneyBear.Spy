using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Autofac;

namespace HoneyBear.Spy.Sample.Host.Web.Infrastructure.IoC
{
    internal class WebApiAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => GlobalConfiguration.Configuration.Routes)
                .AsSelf();

            builder
                .Register(c => GlobalConfiguration.Configuration.Filters)
                .AsSelf();

            builder
                .Register(c => GlobalConfiguration.Configuration)
                .AsSelf();

            builder
                .Register(c => GlobalConfiguration.Configuration.Formatters)
                .AsSelf();

            builder
                .Register(
                    c =>
                    {
                        var httpContext = HttpContext.Current;
                        if (HttpContext.Current == null)
                            httpContext = new HttpContext(c.Resolve<SimpleWorkerRequest>());
                        return new HttpContextWrapper(httpContext);
                    })
                .As<HttpContextBase>();

            builder
                .Register(c => c.Resolve<HttpContextBase>().Request)
                .AsSelf();

            builder
                .Register(c => c.Resolve<HttpRequestBase>().RequestContext)
                .AsSelf();

            builder
                .Register(c => new SimpleWorkerRequest(string.Empty, string.Empty, new StringWriter()))
                .AsSelf();
        }
    }
}