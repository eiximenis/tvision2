using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.Xunit
{
    public class StandardSetupFixtureInitializer<TStartup> : ITuiFixtureInitializer
         where TStartup : class, ITvisionAppStartup
    {
        public HostBuilder GetHostBuilder()
        {
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver()
                .AddTvision2Startup<TStartup>();
                CustomizeSetup(setup);
            });
            return builder;
        }

        protected virtual void CustomizeSetup(Tvision2Setup setup) { }
    }
}