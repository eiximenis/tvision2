using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public abstract class TvControlString<TState> : TvControl<TState>
        where TState : IDirtyObject
    {
        public TvControlString(ISkin skin, IBoxModel boxModel, TState initialState) : base(skin, boxModel, initialState)
        {
        }

        protected sealed override void OnDraw(RenderContext<TState> ctx)
        {
            var str = GetStringToRender(ctx.State);
            ctx.Viewport.DrawStringAt(str, new TvPoint(0, 0), Style.ForeColor, Style.BackColor);
        }

        protected abstract string GetStringToRender(TState state);
    }
}
