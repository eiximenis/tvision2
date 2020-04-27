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
            Core.Engine.ITuiEngine engine = null;
            TvComponent<string> helloWorld = null;

            builder.UseTvision2(setup =>
            {
                setup.UseDotNetConsoleDriver();
                setup.Options.UseStartup((sp, tui) =>
                {
                    engine = tui;
                    helloWorld = new TvComponent<string>("Tvision2 rocks!");
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State, TvPoint.Zero, new TvColorPair(TvColor.Blue, TvColor.Yellow));
                    });
                    helloWorld.AddStateBehavior(s => "Hello world " + new Random().Next(1, 1000) + "    ");
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 10), 30));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 11), 30));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(20, 13), 30));
                    tui.UI.Add(helloWorld);
                    /*
                    for (var x = 0; x<10; x++)
                    {
                        var c = new TvComponent<String>($"{x}");
                        c.AddDrawer(ctx => ctx.DrawStringAt(ctx.GetParentState<string>() + " " + ctx.State, TvPoint.Zero, new TvColorPair(TvColor.Blue, TvColor.Yellow)));
                        c.AddViewport(new Viewport(TvPoint.FromXY(10, 11 + x), 30));
                        tui.UI.AddAsChild(c, helloWorld,  opt =>
                        {
                            opt.RetrieveParentStatusChanges();
                        });
                    }
                    return;
                    */
                    return Task.CompletedTask;
                });
            }).UseConsoleLifetime();

            /*
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                helloWorld.SetState("Again!");
            });
            */

            await builder.RunTvisionConsoleApp();
        }
    }
}
