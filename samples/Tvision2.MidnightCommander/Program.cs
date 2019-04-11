using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Tvision2.Controls.Styles;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.DependencyInjection;

using Tvision2.MidnightCommander.Stores;

namespace Tvision2.MidnightCommander
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver(opt => 
                    opt.Configure()
                        .OnLinux(lo => lo.UseDirectAccess(dop => dop.UseTrueColor())))
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
