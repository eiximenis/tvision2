using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public abstract class TvControlString<TState> : TvControlSingle<TState>
    {
        public TvControlString(TState state, IControlData data) : base(state, data)
        {
        }

        protected sealed override void OnDraw(RenderContext<TState> ctx)
        {
            var str = GetStringToRender(ctx.State);
            ctx.Viewport.DrawStringAt(str, new TvPoint(0, 0), ctx.Style.ForeColor, ctx.Style.BackColor);
        }

        protected abstract string GetStringToRender(TState state);
    }
}
