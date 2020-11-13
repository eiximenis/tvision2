using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tvision2.Blazor.Extensions;
using Microsoft.Extensions.Hosting;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.DependencyInjection;
using Tvision2.Controls.Styles;
using Tvision2.MidnightCommander;
using Tvision2.MidnightCommander.Stores;

namespace Tvision2.BlazorDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            RemoteFileListReducers.RemoteUrl = "http://localhost:5000/files";
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.EnableTvision2(s =>
                    s.UseBlazor(opt => opt.WithPalette(p => p.UseBasicColorMode()))
                    .UseViewportManager()
                    .UseLayoutManager()
                    .AddTvDialogs()
                    .AddTvision2Startup<Startup>()
                    .AddTvControls(options => options.ConfigureSkins(sk => sk.AddMcStyles()))
                    .AddStateManager(sm =>
                    {
                        var ls = sm.AddStore<FileListStore, FileList>("left", new FileListStore(FileList.Empty));
                        ls.AddReducer(RemoteFileListReducers.FileListActions);
                        var rs = sm.AddStore<FileListStore, FileList>("right", new FileListStore(FileList.Empty));
                        rs.AddReducer(RemoteFileListReducers.FileListActions);
                        var gs = sm.AddStore<GlobalStore, GlobalState>("GlobalStore", new GlobalStore());
                        gs.AddReducer(RemoteFileListReducers.FileActions);
                    }));
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
        }
    }
}
