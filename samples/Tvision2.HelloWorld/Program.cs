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
            TvComponent<int> helloWorld = null;

            builder.UseTvision2(setup =>
            {
                setup.UseDotNetConsoleDriver();
                setup.Options.UseStartup((sp, tui) =>
                {
                    engine = tui;
                    helloWorld = new TvComponent<int>(0, "label");
                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.DrawStringAt($"Current value: {ctx.State}".PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero, new TvColorPair(TvColor.Blue, TvColor.Yellow));
                    });

                    helloWorld
                        .IfViewport(v => v.Bounds.Cols < 2)
                        .AddDrawer(ctx =>
                        {
                            ctx.DrawStringAt("*", TvPoint.Zero, new TvColorPair(TvColor.Red, TvColor.Green));
                        });

                    helloWorld
                        .IfViewport(v => v.Bounds.Cols >= 2 && v.Bounds.Cols <= 5)
                        .AddDrawer(ctx =>
                        {
                            ctx.DrawStringAt(ctx.State.ToString().PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero, new TvColorPair(TvColor.Blue, TvColor.Yellow));
                        });

                    helloWorld.AddStateBehavior(s => new Random().Next(1, 1000));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 10), 1));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 11), 5));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(20, 13), 30));

                    tui.UI.Add(helloWorld);
                    return Task.CompletedTask;
                });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }
    }
}
