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
        class StringAndColor
        {
            public string Text { get; set; }
            public TvColor Fore { get; set; }
        }

        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver(opt =>
                {
                    opt.Configure()
                        .OnLinux(l => l.UseAnsi().EnableTrueColor())
                        .OnWindows(w => w.UseAnsi());
                });
                setup.Options.UseStartup((sp, tui) =>
                {
                    var state = new StringAndColor()
                    {
                        Fore = TvColor.FromRGB(0, 0, 0),
                        Text = "Tvision2 rocks!"
                    };
                    var helloWorld = new TvComponent<StringAndColor>(state);
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State.Text, TvPoint.Zero, new TvColorPair(ctx.State.Fore, TvColor.FromRGB(0, 0, 0)));
                    });

                    helloWorld.AddBehavior(state =>
                    {
                        var (red, green, blue) = state.Fore.Rgb;
                        if (red < 255) red++;
                        else if (green < 255) green++;
                        else if (blue < 255) blue++;
                        else red = green = blue = 0;
                        state.Fore = TvColor.FromRGB((byte)red, (byte)green, (byte)blue);

                        state.Text = $"Hello World with ({red}, {green},{blue})        ";
                        return true;
                    });

                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 10), 30));
                    tui.UI.Add(helloWorld);
                    return Task.CompletedTask;
                });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }
    }
}
