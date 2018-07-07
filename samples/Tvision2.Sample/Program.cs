using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Checkbox;
using Tvision2.Controls.Label;
using Tvision2.Controls.Styles;
using Tvision2.Controls.Textbox;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.DependencyInjection;

namespace Tvision2.Sample
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            /*
            var tui = new TuiEngineBuilder()
                .UsePlatformConsoleDriver()
                .UseViewportManager()
                .AddTvControls()
                .AddStateManager(mgr =>
                {
                    mgr.AddStore<TasksStore, TasksList>("Tasks", new TasksStore())
                        .AddReducer(TasksReducers.AddTask);
                })
                .AddSkinSupport(sb =>
                {
                    sb.AddMcStyles();
                })
                .Build();

            */

            var builder = new HostBuilder();
            builder.UseTvision2(setup =>
            {
                setup.UsePlatformConsoleDriver()
                    .AddTvision2Startup<Startup>()
                    .AddTvControls()
                    .AddSkinSupport(sb =>
                    {
                        sb.AddMcStyles();
                    });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
            return 1;
        }
    }
}
