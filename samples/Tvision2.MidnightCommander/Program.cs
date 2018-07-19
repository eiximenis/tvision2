using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.DependencyInjection;
using Tvision2.Controls.Styles;
using Tvision2.Core.Engine;
using Tvision2.Debug;
using System.Text;
using Tvision2.MidnightCommander.Stores;

namespace Tvision2.MidnightCommander
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UseDotNetConsoleDriver()
                    .UseViewportManager()
                    .UseLayoutManager()
                    //.UseDebug(opt =>
                    //{
                    //    opt.UseDebugFilter(c => c.Name.StartsWith("TvControl"));
                    //})
                    .AddTvision2Startup<Startup>()
                    .AddTvControls()
                    .AddSkinSupport(sb =>
                    {
                        sb.AddMcStyles();
                    })
                    .AddStateManager(sm =>
                    {
                        sm.AddStore<FileListStore, FileList>("left", new FileListStore(new FileList()));
                        sm.AddStore<FileListStore, FileList>("right", new FileListStore(new FileList()));
                    });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }
    }
}
