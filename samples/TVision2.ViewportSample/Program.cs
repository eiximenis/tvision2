using System;
using System.Threading.Tasks;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace TVision2.ViewportSample
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var tui = new TuiEngineBuilder()
                .UsePlatformConsoleDriver()
                .UseViewportManager()
                .Build();

            var asterisk = new TvComponent<string>("*");

            asterisk.UseDrawer(ctx =>
            {
                ctx.DrawStringAt(ctx.State, new TvPoint(0, 0), ConsoleColor.White, ConsoleColor.Black);
            });

            asterisk.AddViewport(new Viewport(new TvPoint(10, 10), 1, 1));
            asterisk.AddViewport(new Viewport(new TvPoint(12, 12), 1, 1));
            var guid = asterisk.AddViewport(new Viewport(new TvPoint(13, 20), 1, 1));
            tui.UI.Add(asterisk);
            tui.Start();

            // We update the viewports every 2s. We update 2 of 3 viewports, so:
            // we should see two asterisks moving right-to-left and one more fixed at 12,12
            for (int idx = 0; idx < 9; idx++)
            {
                await Task.Delay(2000);
                asterisk.UpdateViewport(asterisk.Viewport.Translate(new TvPoint(0, -1)));
                asterisk.UpdateViewport(guid, asterisk.GetViewport(guid).Translate(new TvPoint(0, -1)));
            }

            return 0;
        }
    }
}
