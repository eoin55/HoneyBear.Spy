using Topshelf;
using Topshelf.Autofac;

namespace HoneyBear.Spy.Sample.Host.WindowsService
{
    internal class Program
    {
        private static void Main() =>
            HostFactory
                .Run(
                    config =>
                    {
                        config.UseAutofacContainer(IoCBootstrapper.Init());

                        config
                            .Service<HostService>(
                                s =>
                                {
                                    s.ConstructUsingAutofacContainer();
                                    s.WhenStarted(h => h.Start());
                                    s.WhenStopped(h => h.Stop());
                                });
                    });
    }
}
