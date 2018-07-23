using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Label
{
    public class TvLabel : TvControl<LabelState>
    {

        public TvLabel(ISkin skin, IViewport boxModel, LabelState state) : base(skin, boxModel, state)
        {
        }


        protected override void OnDraw(RenderContext<LabelState> context)
        {
            var state = context.State;
            var currentcols = context.Viewport.Columns;
            var focused = Metadata.IsFocused;
            var style = focused ? CurrentStyles.GetStyle("focused") : CurrentStyles.GetStyle("");
            var value = string.Format("{0}{1}{2}",
                focused ? ">" : "",
                state.Text.ToString() ?? "",
                focused ? "<" : "");
            context.Fill(style.BackColor);
            context.DrawStringAt(value, new TvPoint(0, 0), style.ForeColor, style.BackColor);
        }
    }
}
