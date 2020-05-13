using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Core.Components.Behaviors;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tvision2.HelloWorld
{
    class StringHolder
    {
        public string Value { get; set; }
        public StringHolder(string s)
        {
            Value = s;
        }
    }
    class Ufo : ITvBehavior<StringHolder>
    {
     
        public TvComponent<string> Label { get; set; }

        public bool Update(BehaviorContext<StringHolder> updateContext)
        {
            if (Label != null)
            {
                updateContext.State.Value = Label.State + "!!!!!";
                return true;
            }

            return false;
        }
    }

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            Core.Engine.ITuiEngine engine = null;
            TvComponent<string> helloWorld = null;

            builder.UseTvision2(setup =>
            {
                setup.UseDotNetConsoleDriver();
                setup.Options.UseStartup((sp, tui) =>
                {
                    engine = tui;
                    helloWorld = new TvComponent<string>("Tvision2 rocks!", "label");
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State, TvPoint.Zero, new TvColorPair(TvColor.Blue, TvColor.Yellow));
                    });
                    helloWorld.AddStateBehavior(s => "Hello world " + new Random().Next(1, 1000) + "    ");
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 10), 30));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 11), 30));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(20, 13), 30));

                    var label2 = new TvComponent<StringHolder>(new StringHolder("****"), "label2");
                    label2.AddBehavior(new Ufo(), m => m.AddDependency(u => u.Label, "label"));
                    label2.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt(ctx.State.Value, TvPoint.Zero, new TvColorPair(TvColor.Red, TvColor.White));
                    });
                    label2.AddViewport(new Viewport(TvPoint.FromXY(20, 15), 30));
                    tui.UI.Add(helloWorld);
                    tui.UI.Add(label2);
                    return Task.CompletedTask;
                });
            }).UseConsoleLifetime();

            /*
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                helloWorld.SetState("Again!");
            });
            */

            await builder.RunTvisionConsoleApp();
        }
    }
}
