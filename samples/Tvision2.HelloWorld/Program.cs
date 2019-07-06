using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.HelloWorld
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UseDotNetConsoleDriver();
                setup.Options.UseStartup((sp, tui) =>
                {
                    var helloWorld = new TvComponent<string>("Tvision2 rocks!");
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State, TvPoint.Zero, new TvColorPair(TvColor.Blue, TvColor.Yellow)) ;
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
