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
                ctx.DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero, new TvColorPair(TvColor.Blue, TvColor.Yellow));
            });

            helloWorld.AddStateBehavior(s => "Hello Blazor! :) " + Guid.NewGuid().ToString());

            helloWorld.AddViewport(new Viewport(TvPoint.FromXY(3, 10), 70));
            tui.UI.Add(helloWorld);
            return Task.CompletedTask;
        }
    }
}
