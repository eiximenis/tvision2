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
                    style.DesiredStandard(TvColor.White, TvColor.Blue);
                    style.DesiredFocused(TvColor.Black, TvColor.Cyan);
                });
                
                sb.AddStyle("tvlist", style =>
                {
                    style.DesiredStandard(TvColor.White, TvColor.Blue);
                    style.DesiredFocused(TvColor.White, TvColor.Blue, CharacterAttributeModifiers.BackgroundBold);
                    style.DesiredAlternate(TvColor.Black, TvColor.Cyan);
                    style.DesiredAlternateFocused(TvColor.Black, TvColor.White);
                });

                sb.AddStyle("tvtextbox", style =>
                {
                    style.DesiredStandard(TvColor.Black, TvColor.White);
                    style.DesiredFocused(TvColor.White, TvColor.Black);
                });

                sb.AddStyle("tvdialog", style =>
                {
                    style.DesiredStandard(TvColor.Black, TvColor.White);
                    style.DesiredFocused(TvColor.White, TvColor.Black);
                });

                sb.AddStyle("tvmenubar", style =>
                {
                    style.DesiredFocused(TvColor.White, TvColor.Black, CharacterAttributeModifiers.Bold);
                    style.DesiredStandard(TvColor.Black, TvColor.Cyan);
                    style.DesiredAlternate(TvColor.Yellow, TvColor.Cyan, CharacterAttributeModifiers.Bold);
                    style.DesiredAlternateFocused(TvColor.Yellow, TvColor.Black, CharacterAttributeModifiers.Bold);
                });
            });

            return builder;
        }
    }
}
