using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tvision2.Xunit
{
    public class TuiFixture<TInit> 
        where TInit : ITuiFixtureInitializer, new()
    {
        public IServiceProvider ServiceProvider { get; }

        public TuiFixture()
        {
            var init = new TInit();
            var builder = init.GetHostBuilder();
            // builder.RunTvisionConsoleApp();
            ServiceProvider = builder.Build().Services;
        }

        public void Run(Action<IServiceProvider> code)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                code.Invoke(scope.ServiceProvider);
            }
        }

        public T GetOfType<T>()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                return scope.ServiceProvider.GetService<T>();
            }
        }
    }
}
