using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.BlazorDemo
{
    public class Startup : ITvisionAppStartup
    {
        Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {
            var helloWorld = new TvComponent<string>("Hello Blazor! :)");
            helloWorld.AddDrawer(ctx =>
            {
                var rnd = new Random();
                var red = (byte)rnd.Next(0, 256);
                var blue = (byte)rnd.Next(0, 256);
                var green = (byte)rnd.Next(0, 256);
                var red2 = (byte)rnd.Next(0, 256);
                var blue2 = (byte)rnd.Next(0, 256);
                var green2 = (byte)rnd.Next(0, 256);


                ctx.DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero, new TvColorPair(TvColor.FromRGB(red, green, blue), TvColor.FromRGB(red2, green2, blue2)));
            });

            helloWorld.AddStateBehavior(s => "Hello Blazor! :) " + Guid.NewGuid().ToString());

            helloWorld.AddViewport(new Viewport(TvPoint.FromXY(3, 10), 70));
            tui.UI.Add(helloWorld);
            return Task.CompletedTask;
        }
    }
}
