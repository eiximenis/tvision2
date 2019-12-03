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
                        .OnLinux(l => l.UseDirectAccess(da => da.UseTrueColor().WithBuiltInSequences()))
                        .OnWindows(w => w.EnableAnsiSequences());
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
                    helloWorld.AddBehavior(ctx =>
                    {
                        var r = new Random();
                        var (red, green, blue) = ctx.State.Fore.Rgb;
                        ctx.State.Fore = TvColor.FromRGB((byte)((red + 1) % 256), (byte)((green + 1) % 256), (byte)((blue + 1) % 256));
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
