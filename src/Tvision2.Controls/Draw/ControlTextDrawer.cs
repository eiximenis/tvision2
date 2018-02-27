using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Components.Render;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Draw
{
    public class ControlTextDrawer<T> : ConverterDrawer<T, string>
    {
        public ControlTextDrawer(Func<T, string> converter) : base(converter)
        {
        }

        protected override void Draw(RenderContext<string> context)
        {
            var focused = context.Style.ContainsClass("focused");

            var value = string.Format("{0}{1}{2}{3}{4}",
                new string(' ', context.Style.PaddingLeft),
                focused ? ">" : "",
                context.State.ToString() ?? "",
                focused ? "<" : "",
                new string(' ', context.Style.PaddingRight));

            context.Viewport.DrawStringAt(value, new TvPoint(0, 0), context.Style.ForeColor, context.Style.BackColor);
        }

    }
}