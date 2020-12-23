using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

using Tvision2.Controls.Styles;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Engine;
using Tvision2.DependencyInjection;

namespace Tvision2.ControlsGallery
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver(options => 
                        options.Configure(c => c
                            .EnableMouse()    
                            .UseBackColor(TvColor.Black))
                            .OnWindows(w => w.UseAnsi())
                            .OnLinux(l => l
                                .UseNCurses()))
                    .UseViewportManager()
                    .UseLayoutManager()
                    .AddTvDialogs()
                    //.UseDebug(opt =>
                    //{
                    //    opt.UseDebugFilter(c => c.Name.StartsWith("TvControl"));
                    //})
                    .AddTvision2Startup<Startup>()
                    .AddTvControls(options => options.EnableMouseManager().ConfigureSkins(sk => sk.AddMcStyles()));
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }
    }
}
