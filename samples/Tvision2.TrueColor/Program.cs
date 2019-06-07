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
                {
                    opt.Configure()
                        .OnLinux(l => l.UseDirectAccess(da => da.UseTrueColor().WithBuiltInSequences()))
                        .OnWindows(w => w.EnableAnsiSequences());
                });
                setup.Options.UseStartup((sp, tui) =>
                {
                    var cm = sp.GetService<IColorManager>();
                    
                    (byte fr, byte fg, byte fb, CharacterAttribute attr) cs = (0, 0, 0, default(CharacterAttribute));
                    cs.attr = cm.BuildAttributeFor(TvColor.FromRGB(cs.fr, cs.fg, cs.fb), TvColor.FromRGB(0, 0, 0));
                    var helloWorld = new TvComponent<string>("Tvision2 rocks!");
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State, TvPoint.Zero, cs.attr);
                    });
                    helloWorld.AddBehavior(ctx =>
                    {
                        var r = new Random();
                        cs.fr = (byte)((cs.fr +1) % 256);
                        cs.fb = (byte)((cs.fb +1) % 256);
                        cs.fg = (byte)((cs.fg +1) % 256);
                        cs.attr = cm.BuildAttributeFor(TvColor.FromRGB(cs.fr, cs.fg, cs.fb), TvColor.FromRGB(0, 0, 0));
                        return true;
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
