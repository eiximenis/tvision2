using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public abstract class TvControlSingle<TState> : TvControl<TState>
    {
        public TvControlSingle(TState state, IControlData data) : base(state, data)
        {
        }

        protected sealed override void AddComponentElements(TvComponent<TState> cmp)
        {
            cmp.AddDrawer(OnDraw);
            AddAdditionalComponentElements(cmp);
        }

        protected virtual void AddAdditionalComponentElements(TvComponent<TState> cmp)
        {
        }

        protected abstract void OnDraw(RenderContext<TState> ctx);
    }
}
