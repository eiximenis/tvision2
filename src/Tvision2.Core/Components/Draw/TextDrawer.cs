using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Render
{
    public class TextDrawer<T> : ActionDrawer<T, TextDrawerOptions>
    {

        public TextDrawer(Action<TextDrawerOptions> optionsAction = null) : base(DrawFunc, optionsAction)
        {
        }

        private static void DrawFunc(RenderContext<T> context, TextDrawerOptions options)
        {
            var value = string.Format("{0}{1}{2}",
                new string(' ', context.Style.PaddingLeft),
                context.State.ToString() ?? "",
                new string(' ', context.Style.PaddingRight));

            context.Viewport.DrawAt(value, new TvPoint(0, 0), context.Style.ForeColor, context.Style.BackColor);
        }
    }
}
