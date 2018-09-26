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
            Metadata.CanFocus = false;
        }


        protected override void OnDraw(RenderContext<LabelState> context)
        {
            var state = context.State;
            var currentcols = context.Viewport.Columns;
            var focused = Metadata.IsFocused;
            var pairIdx = CurrentStyle.Standard;
            var value = state.Text.ToString() ?? "";
            context.Fill(pairIdx);
            context.DrawStringAt(value, new TvPoint(0, 0), pairIdx);
        }
    }
}
