using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core;
using Tvision2.Core.Render;
using Tvision2.Core.Colors;
using System.Threading.Tasks;
using Tvision2.DependencyInjection;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.StyledHelloWorld
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            ITuiEngine engine = null;
            TvComponent<string> helloWorld = null;

            builder.UseTvision2(setup =>
            {
                setup.UseDotNetConsoleDriver();
                setup.AddStyles(sb => AddApplicationStyles(sb));
                setup.Options.UseStartup((sp, tui) =>
                {
                    engine = tui;
                    helloWorld = new TvComponent<string>("Hello world");
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 10), 1));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 11), 5));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(20, 13), 30));

                    tui.UI.Add(helloWorld);
                    return Task.CompletedTask;
                });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }

        private static void AddApplicationStyles(ISkinManagerBuilder sb)
        {
            sb.AddDefaultSkin(skin =>  {
                skin.AddBaseStyle(bs => bs.Default().DesiredStandard(sd => sd.UseBackground(TvColor.Red).UseForeground(TvColor.Black)));
                skin.AddStyle("text", bs => bs.Default().DesiredStandard(sd => sd.UseBackground(TvColor.Blue).UseForeground(TvColor.Yellow)));
            });
        }
    }
}

