using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.TrueColor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver(opt =>
                    opt.Configure().OnLinux(l => l.UseDirectAccess(da => da.UseTrueColor().WithBuiltInSequences())));
                setup.Options.UseStartup((sp, tui) =>
                {
                    var cm = sp.GetService<IColorManager>();
                    var attr = cm.BuildAttributeFor(TvColor.FromRGB(200, 10, 20), TvColor.FromRGB(0, 0, 100));
                    var helloWorld = new TvComponent<string>("Tvision2 rocks!");
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State, TvPoint.Zero, attr) ;
                    });
                    helloWorld.AddViewport(new Viewport(new TvPoint(10, 10), 30));
                    tui.UI.Add(helloWorld);
                    return Task.CompletedTask;
                });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }
    }
}