using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Styles;
using Tvision2.Styles.Backgrounds;

namespace Tvision2.Controls.Styles
{
    public static class TuiEngineBuilderExtensions_StylesMc
    {
        public static ISkinManagerBuilder AddMcStyles(this ISkinManagerBuilder builder)
        {
            builder.AddSkin("Mc", sb =>
            {

                sb.AddBaseStyle(style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Blue)
                    )
                        .DesiredFocused(o =>
                            o.UseForeground(TvColor.Black)
                            .UseBackground(TvColor.Cyan)
                    );
                });

                sb.AddStyle("tvlist", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Blue)
                    )
                    .DesiredFocused(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Blue)
                         .AddModifier(CharacterAttributeModifiers.BackgroundBold)
                    )
                    .DesiredAlternate(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.Cyan)
                    )
                    .DesiredAlternateFocused(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.White)
                    );

                    style
                        .When(p =>  p.ColorMode ==ColorMode.Direct)
                        .DesiredStandard(o =>
                            o.UseBackground(() => new VerticalGradientBackgroundProvider(
                                TvColor.FromRGB(0, 200, 100), TvColor.FromRGB(50, 50, 50)))
                        );

                    style
                        .When(p => p.ColorMode == ColorMode.Palettized)
                        .DesiredStandard(o =>
                            o.UseBackground(() => new VerticalGradientBackgroundProvider(
                                TvColor.FromRGB(0, 255, 0), TvColor.FromRGB(0, 200, 0)))
                            );

                });

                sb.AddStyle("tvtextbox", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.White)
                    )
                    .DesiredFocused(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Black)
                    );
                });

                sb.AddStyle("tvdialog", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.White)
                    )
                    .DesiredFocused(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Black)
                    );

                    style
                        .When(p =>  p.ColorMode ==ColorMode.Direct ||  p.ColorMode == ColorMode.Palettized )
                        .DesiredStandard(o =>
                            o.UseBackground(() => new VerticalGradientBackgroundProvider(
                                TvColor.FromRGB(255, 128, 128), TvColor.FromRGB(128, 255, 255)))
                        );
                });

                sb.AddStyle("tvmenubar", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Black)
                         .AddModifier(CharacterAttributeModifiers.Bold)
                    )
                    .DesiredFocused(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.Cyan)
                    )
                    .DesiredAlternate(o =>
                        o.UseForeground(TvColor.Yellow)
                         .UseBackground(TvColor.Cyan)
                         .AddModifier(CharacterAttributeModifiers.Bold)
                    )
                    .DesiredAlternateFocused(o =>
                        o.UseForeground(TvColor.Yellow)
                         .UseBackground(TvColor.Black)
                         .AddModifier(CharacterAttributeModifiers.Bold)
                    );
                });
            });

            return builder;
        }
    }
}
