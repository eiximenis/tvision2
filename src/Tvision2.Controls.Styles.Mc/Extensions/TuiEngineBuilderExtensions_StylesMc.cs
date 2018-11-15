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
                    style.DesiredStandard(TvisionColor.White, TvisionColor.Blue);
                    style.DesiredFocused(TvisionColor.Black, TvisionColor.Cyan);
                });
                
                sb.AddStyle("tvlist", style =>
                {
                    style.DesiredStandard(TvisionColor.White, TvisionColor.Blue);
                    style.DesiredFocused(TvisionColor.White, TvisionColor.Blue, CharacterAttributeModifiers.BackgroundBold);
                    style.DesiredAlternate(TvisionColor.Black, TvisionColor.Cyan);
                    style.DesiredAlternateFocused(TvisionColor.Black, TvisionColor.White);
                });

                sb.AddStyle("tvtextbox", style =>
                {
                    style.DesiredStandard(TvisionColor.Black, TvisionColor.White);
                    style.DesiredFocused(TvisionColor.White, TvisionColor.Black);
                });

                sb.AddStyle("tvdialog", style =>
                {
                    style.DesiredStandard(TvisionColor.Black, TvisionColor.White);
                    style.DesiredFocused(TvisionColor.White, TvisionColor.Black);
                });

                sb.AddStyle("tvmenubar", style =>
                {
                    style.DesiredFocused(TvisionColor.White, TvisionColor.Cyan, CharacterAttributeModifiers.Bold);
                    style.DesiredStandard(TvisionColor.Black, TvisionColor.Cyan);
                });
            });

            return builder;
        }
    }
}
