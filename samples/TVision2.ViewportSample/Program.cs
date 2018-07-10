using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts.Grid;

namespace TVision2.ViewportSample
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {

            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver()
                    .UseDebug()
                    .UseViewportManager()
                    .UseLayoutManager();

                setup.Options.UseStartup(async (sp, tui) =>
                {

                    var grid = new TvGrid(tui.UI, new GridState(2, 2), "MyGrid");
                    grid.AsComponent().AddViewport(new Viewport(new TvPoint(0, 0), 20, 10));

                    var asterisk = new TvComponent<string>("*");
                    asterisk.UseDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State, new TvPoint(0, 0), ConsoleColor.White, ConsoleColor.Black);
                    });

                    var dollar = new TvComponent<string>("$");
                    dollar.UseDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State, new TvPoint(1, 2), ConsoleColor.Red, ConsoleColor.Black);
                    });

                    grid.AddChild(asterisk, 1, 1);
                    grid.AddChild(dollar, 0, 0);
                    tui.UI.Add(grid.AsComponent());

                });
            }).UseConsoleLifetime();


            await builder.RunTvisionConsoleApp();
            return 1;

            /*
            // We update the viewports every 2s. We update 2 of 3 viewports, so:
            // we should see two asterisks moving right-to-left and one more fixed at 12,12
            for (int idx = 0; idx < 9; idx++)
            {

            }
            */
        }
    }
}
