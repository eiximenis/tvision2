    using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
    using Tvision2.ConsoleDriver.ColorTranslators;
    using Tvision2.Controls.Styles;
using Tvision2.Core;
    using Tvision2.Core.Colors;
    using Tvision2.Core.Engine;
using Tvision2.DependencyInjection;

using Tvision2.MidnightCommander.Stores;

namespace Tvision2.MidnightCommander
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            bool useFullColor = false;
            foreach (var arg in args)
            {
                if (arg == "--fullcolor") { useFullColor = true; }
            }
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver(opt => 
                    opt.Configure()
                        // Linux specific config
                        .OnLinux(lo =>
                        {
                            // We want to setup our palette
                            lo.UsePalette(p =>
                            {
                                // Will init the palette using our terminal name (currently only xterm-256color is supported)
                                p.LoadFromTerminalName();
                                // We want to be able to use RGB colors, but if we are in palette mode (no full direct color)
                                // we need to setup a translator that translates any RGB color in a palette color. 
                                p.TranslateRgbColorsWith(new  InterpolationPaletteTranslator());
                            });
                            if (useFullColor)
                            {
                                // We want to use full color if possible. We will
                                // be able to translate between palettized colors and rgb ones because we
                                // used UsePalette and load a palette.
                                lo.UseDirectAccess(dop => dop.UseTrueColor());
                            }
                        })
                        // Windows specific config
                        .OnWindows(w =>
                        {
                            if (useFullColor)
                            {
                                // We want to use ANSI sequences, allowing full color also. This is only
                                // available in Win10. If running in older windows, application will be in
                                // basic color mode.
                                w.EnableAnsiSequences();
                            }
                        })
                     )
                    .UseViewportManager()
                    .UseLayoutManager()
                    .AddTvDialogs()
                    //.UseDebug(opt =>
                    //{
                    //    opt.UseDebugFilter(c => c.Name.StartsWith("TvControl"));
                    //})
                    .AddTvision2Startup<Startup>()
                    .AddTvControls(sk => sk.AddMcStyles())
                    .AddStateManager(sm =>
                    {
                        var ls = sm.AddStore<FileListStore, FileList>("left", new FileListStore(FileList.Empty));
                        ls.AddReducer(FileListReducers.FileListActions);
                        var rs = sm.AddStore<FileListStore, FileList>("right", new FileListStore(FileList.Empty));
                        rs.AddReducer(FileListReducers.FileListActions);
                        var gs = sm.AddStore<GlobalStore, GlobalState>("GlobalStore", new GlobalStore());
                        gs.AddReducer(FileListReducers.FileActions);
                    });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }
    }
}
