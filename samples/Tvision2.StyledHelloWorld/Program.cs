using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core;
using Tvision2.Core.Render;
using Tvision2.Core.Colors;
using System.Threading.Tasks;
using Tvision2.DependencyInjection;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.StyledHelloWorld
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            ITuiEngine? engine = null;
            TvComponent<string>? helloWorld = null;
            TvComponent<string>? helloWorld2= null;
            TvComponent<string>? helloWorld3 = null;
            TvComponent<string>? helloWorld4 = null;
            TvComponent<string>? helloWorld5 = null;
            TvComponent<string>? helloWorld6 = null;
            TvComponent<string>? helloWorld7 = null;
            TvComponent<string>? helloWorld8 = null;
            TvComponent<string>? helloWorld9 = null;

            builder.UseTvision2(setup =>
            {
                setup.UseDotNetConsoleDriver();
                setup.AddStyles(AddApplicationStyles);
                setup.Options.UseStartup((sp, tui) =>
                {
                    engine = tui;
                    helloWorld = new TvComponent<string>("Single").WithBorder(BorderValue.Single(), "text");
                    helloWorld2 = new TvComponent<string>("Double").WithBorder(BorderValue.Double(), "text");
                    helloWorld3 = new TvComponent<string>("Double, Single")
                        .WithBorder(BorderValue.FromHorizontalAndVertical(BorderType.Double, BorderType.Single), "text");
                    helloWorld4 = new TvComponent<string>("Single, Double")
                        .WithBorder(BorderValue.FromHorizontalAndVertical(BorderType.Single, BorderType.Double), "text");
                    helloWorld5 = new TvComponent<string>("Fill")
                        .WithBorder(BorderValue.FromHorizontalAndVertical(BorderType.Fill, BorderType.Fill), "text");
                    helloWorld6 = new TvComponent<string>("Fill, Single")
                        .WithBorder(BorderValue.FromHorizontalAndVertical(BorderType.Fill, BorderType.Single), "text");
                    helloWorld7 = new TvComponent<string>("Fill, Double")
                        .WithBorder(BorderValue.FromHorizontalAndVertical(BorderType.Fill, BorderType.Double), "text");
                    helloWorld8 = new TvComponent<string>("Single, Fill")
                        .WithBorder(BorderValue.FromHorizontalAndVertical(BorderType.Single, BorderType.Fill), "text");
                    helloWorld9 = new TvComponent<string>("Double, Fill")
                        .WithBorder(BorderValue.FromHorizontalAndVertical(BorderType.Double, BorderType.Fill), "text");


                    helloWorld.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld2.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld3.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld4.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld5.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld6.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld7.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld8.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });

                    helloWorld9.AddDrawer(ctx =>
                    {
                        ctx.Styled("text").DrawStringAt(ctx.State.PadRight(ctx.Viewport.Bounds.Cols), TvPoint.Zero);
                    });




                    // helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 10), 1));
                    // helloWorld.AddViewport(new Viewport(TvPoint.FromXY(10, 11), 5));
                    helloWorld.AddViewport(new Viewport(TvPoint.FromXY(20, 1), TvBounds.FromRowsAndCols(3, 30)));
                    helloWorld2.AddViewport(new Viewport(TvPoint.FromXY(20, 5), TvBounds.FromRowsAndCols(3, 30)));
                    helloWorld3.AddViewport(new Viewport(TvPoint.FromXY(20, 9), TvBounds.FromRowsAndCols(3, 30)));
                    helloWorld4.AddViewport(new Viewport(TvPoint.FromXY(20, 13), TvBounds.FromRowsAndCols(3, 30)));
                    helloWorld5.AddViewport(new Viewport(TvPoint.FromXY(20, 17), TvBounds.FromRowsAndCols(3, 30)));

                    helloWorld6.AddViewport(new Viewport(TvPoint.FromXY(52, 1), TvBounds.FromRowsAndCols(3, 30)));
                    helloWorld7.AddViewport(new Viewport(TvPoint.FromXY(52, 5), TvBounds.FromRowsAndCols(3, 30)));
                    helloWorld8.AddViewport(new Viewport(TvPoint.FromXY(52, 9), TvBounds.FromRowsAndCols(3, 30)));
                    helloWorld9.AddViewport(new Viewport(TvPoint.FromXY(52, 13), TvBounds.FromRowsAndCols(3, 30)));

                    tui.UI.Add(helloWorld);
                    tui.UI.Add(helloWorld2);
                    tui.UI.Add(helloWorld3);
                    tui.UI.Add(helloWorld4);
                    tui.UI.Add(helloWorld5);
                    tui.UI.Add(helloWorld6);
                    tui.UI.Add(helloWorld7);
                    tui.UI.Add(helloWorld8);
                    tui.UI.Add(helloWorld9);

                    return Task.CompletedTask;
                });
            }).UseConsoleLifetime();
            await builder.RunTvisionConsoleApp();
        }

        private static void AddApplicationStyles(ISkinManagerBuilder sb)
        {
            sb.AddDefaultSkin(skin =>  {
                skin.AddBaseStyle(bs => bs.Default().DesiredStandard(sd => sd.UseBackground(TvColor.Red).UseForeground(TvColor.Black)));
                skin.AddStyle("text", bs => bs.Default()
                    .DesiredStandard(sd => sd.UseBackground(TvColor.Blue).UseForeground(TvColor.Yellow))
                    .DesiredAlternate(sd => sd.UseBackground(TvColor.Black).UseForeground(TvColor.White))
                    .Desired("custom_name", sd => sd.UseBackground(TvColor.Yellow).UseForeground(TvColor.Black))
                );
            });
        }
    }
}

