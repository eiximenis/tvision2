using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.StartupSample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.UseTvision2(opt =>
            {
                opt.UsePlatformConsoleDriver();
            });

            builder.UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }
    }
}
