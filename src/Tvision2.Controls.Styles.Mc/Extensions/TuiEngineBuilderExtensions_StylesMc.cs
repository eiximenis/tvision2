using Tvision2.Controls.Backgrounds;
using Tvision2.Core.Colors;

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
                    );
                    style.Default().DesiredFocused(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.Cyan)
                    );
                });

                sb.AddStyle("tvlist", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Blue)
                    );
                    style
                        .When(cm => cm == Core.ColorMode.Palettized || cm == Core.ColorMode.Direct)
                        .DesiredStandard(o =>
                            o.UseBackground(() => new VerticalGradientBackgroundProvider(
                                TvColor.FromRGB(0, 200, 100), TvColor.FromRGB(50, 50, 50)))
                        );
                    style.Default().DesiredFocused(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Blue)
                         .AddModifier(CharacterAttributeModifiers.BackgroundBold)
                    );
                    style.Default().DesiredAlternate(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.Cyan)
                    );
                    style.Default().DesiredAlternateFocused(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.White)
                    );
                });

                sb.AddStyle("tvtextbox", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.White)
                    );
                    style.Default().DesiredFocused(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Black)
                    );
                });

                sb.AddStyle("tvdialog", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.White)
                    );
                    style.Default().DesiredFocused(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Black)
                    );
                });

                sb.AddStyle("tvmenubar", style =>
                {
                    style.Default().DesiredStandard(o =>
                        o.UseForeground(TvColor.White)
                         .UseBackground(TvColor.Black)
                         .AddModifier(CharacterAttributeModifiers.Bold)
                    );
                    style.Default().DesiredFocused(o =>
                        o.UseForeground(TvColor.Black)
                         .UseBackground(TvColor.Cyan)
                    );
                    style.Default().DesiredAlternate(o =>
                        o.UseForeground(TvColor.Yellow)
                         .UseBackground(TvColor.Cyan)
                         .AddModifier(CharacterAttributeModifiers.Bold)
                    );
                    style.Default().DesiredAlternateFocused(o =>
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
