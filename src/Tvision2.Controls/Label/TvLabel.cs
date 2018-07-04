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

        private IViewport RequestNewBoxModel(IViewport current, string toRender)
        {
            var needed = toRender.Length;
            if (current.Clipping == ClippingMode.ExpandBoth || current.Clipping == ClippingMode.ExpandHorizontal)
            {
                return needed > current.Columns ? current.ResizeTo(needed, current.Rows) : null;
            }

            return null;
        }

        protected override void OnDraw(RenderContext<LabelState> context)
        {
            var state = context.State;
            var currentcols = context.Viewport.Columns;
            var focused = Style.ContainsClass("focused");
            var value = string.Format("{0}{1}{2}{3}{4}",
                new string(' ', Style.PaddingLeft),
                focused ? ">" : "",
                state.Text.ToString() ?? "",
                focused ? "<" : "",
                new string(' ', Style.PaddingRight));

            context.Fill(Style.BackColor);

            var boxModel = RequestNewBoxModel(context.Viewport, value);
            if (boxModel != null)
            {
                ApplyNewBoxModel(context, boxModel);
            }

            context.DrawStringAt(value, new TvPoint(0, 0), Style.ForeColor, Style.BackColor);
        }
    }
}
