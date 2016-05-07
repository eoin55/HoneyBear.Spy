using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using HoneyBear.Spy.Sample.Host.Web.Controllers;

namespace HoneyBear.Spy.Sample.Host.Web.Infrastructure.WebApi
{
    internal class HttpRouteBootstrapper : IBootstrapper
    {
        private readonly HttpRouteCollection _routes;

        public HttpRouteBootstrapper(
            HttpRouteCollection routes)
        {
            _routes = routes;
        }

        public void Init()
        {
            _routes
                .MapHttpRoute(
                    "GetDefault",
                    "",
                    new {action = "Get", controller = "Home"},
                    new {httpMethod = new HttpMethodConstraint(new HttpMethod("Get"))});
        }
    }
}