    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
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
            bool useNCurses = false;
            bool useBasic = false;
            foreach (var arg in args)
            {
                if (arg == "--fullcolor") { useFullColor = true; }
                if (arg == "--ncurses") { useNCurses = true; }
                if (arg == "--basic")  {  useBasic = true;  }
            }
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver(opt => 
                    opt.Configure()
                        // Linux specific config
                        .OnLinux(lo =>
                        {
                            if (!useNCurses)
                            {
                                // We want to setup our palette
                                if (useBasic)
                                {
                                    lo.UseAnsi().WithPalette(p => p.UseBasicColorMode());
                                }

                                else 
                                {
                                    lo.UseAnsi()
                                        .EnableTrueColor(tc => tc.WithBuiltInSequences())
                                        .WithPalette(palette =>
                                        {
                                            // palette.LoadFromTerminalName("256-grey")
                                                palette.LoadFromTerminalName()
                                                .UpdateTerminal(ConsoleDriver.Common.UpdateTerminalEntries
                                                    .AllButAnsi4bit);
                                            palette.TranslateRgbColorsWith(new  InterpolationPaletteTranslator());
                                        });
                                }
                            }
                            else
                            {
                                if (useBasic)
                                {
                                    lo.UseNCurses().WithPalette(p => p.UseBasicColorMode());
                                }
                                else  lo.UseAnsi().WithPalette(palette =>
                                {
                                    palette.UseBasicColorMode();
                                });
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
                                w.UseAnsi().EnableTrueColor();
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
