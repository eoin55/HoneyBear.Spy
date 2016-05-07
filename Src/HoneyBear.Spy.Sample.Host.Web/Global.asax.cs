using System;
using System.Web;
using HoneyBear.Spy.Sample.Host.Web.Infrastructure.IoC;

namespace HoneyBear.Spy.Sample.Host.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            IoCBootstrapper.Init();
        }
    }
}