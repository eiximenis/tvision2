using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

using Tvision2.Controls.Styles;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Engine;
using Tvision2.DependencyInjection;
using Tvision2.Styles.Backgrounds;

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
                            .OnWindows(w => w.UseAnsi().EnableTrueColor())
                            .OnLinux(l => l
                                .UseNCurses()))
                    //.UseDebug()
                    .UseViewportManager()
                    .UseLayoutManager()
                    .AddStyles(
                        sk => sk.AddDefaultSkin(smb =>
                        {
                            smb.AddStyle("tvgrid", sb =>
                            {
                                sb.Default().DesiredStandard(s =>
                                    s.UseForeground(TvColor.White)
                                    .UseBackground(TvColor.Black));
                                     //.UseBackground(() => new VerticalGradientBackgroundProvider(TvColor.FromRGB(0, 255, 255), TvColor.FromRGB(128, 20, 20))));
                            });
                        })
                    )
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
