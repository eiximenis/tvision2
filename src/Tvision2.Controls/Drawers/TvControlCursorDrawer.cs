using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Drawers
{
    class TvControlCursorDrawer<TState> : ITvDrawer<TState>
    {
        private Action<ICursorContext, TState> _cursorAction;
        private readonly TvControlMetadata _metadata;

        public TvControlCursorDrawer(Action<ICursorContext, TState> cursorAction, TvControlMetadata metadata)
        {
            _cursorAction = cursorAction;
            _metadata = metadata;
        }

        public DrawResult Draw(RenderContext<TState> context)
        {
            if (_metadata.IsFocused)
            {
                _cursorAction(context, context.State);
            }

            return DrawResult.Done;
        }

        DrawResult ITvDrawer.Draw(RenderContext context) => Draw(context as RenderContext<TState>);
    }
}
