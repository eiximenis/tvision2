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
                    style.DesiredStandard(DefaultColorName.White, DefaultColorName.Blue);
                    style.DesiredFocused(DefaultColorName.Black, DefaultColorName.Cyan);
                });
                
                sb.AddStyle("tvlist", style =>
                {
                    style.DesiredStandard(DefaultColorName.White, DefaultColorName.Blue);
                    style.DesiredFocused(DefaultColorName.White, DefaultColorName.Blue, CharacterAttributeModifiers.BackgroundBold);
                    style.DesiredAlternate(DefaultColorName.Black, DefaultColorName.Cyan);
                    style.DesiredAlternateFocused(DefaultColorName.Black, DefaultColorName.White);
                });

                sb.AddStyle("tvtextbox", style =>
                {
                    style.DesiredStandard(DefaultColorName.Black, DefaultColorName.White);
                    style.DesiredFocused(DefaultColorName.White, DefaultColorName.Black);
                });

                sb.AddStyle("tvdialog", style =>
                {
                    style.DesiredStandard(DefaultColorName.Black, DefaultColorName.White);
                    style.DesiredFocused(DefaultColorName.White, DefaultColorName.Black);
                });

                sb.AddStyle("tvmenubar", style =>
                {
                    style.DesiredFocused(DefaultColorName.White, DefaultColorName.Cyan, CharacterAttributeModifiers.Bold);
                    style.DesiredStandard(DefaultColorName.Black, DefaultColorName.Cyan);
                });
            });

            return builder;
        }
    }
}
