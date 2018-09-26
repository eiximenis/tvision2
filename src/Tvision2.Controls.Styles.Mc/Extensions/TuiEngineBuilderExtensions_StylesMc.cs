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
                    style.DesiredStandard(DefaultColorName.Yellow, DefaultColorName.Blue);
                    style.DesiredFocused(DefaultColorName.White, DefaultColorName.Blue);
                });

                sb.AddStyle("tvtextbox", style =>
                {
                    style.DesiredStandard(DefaultColorName.Black, DefaultColorName.White);
                    style.DesiredFocused(DefaultColorName.White, DefaultColorName.Black);
                });
            });

            return builder;
        }
    }
}
