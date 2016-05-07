using System.Web.Http;
using HoneyBear.Spy.Sample.Library;

namespace HoneyBear.Spy.Sample.Host.Web.Controllers
{
    public class HomeController : ApiController
    {
        private readonly Foo _foo;

        public HomeController(Foo foo)
        {
            _foo = foo;
        }

        public IHttpActionResult Get()
        {
            _foo.Bar();

            return Ok("Hello, world!");
        }
    }
}