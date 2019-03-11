using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public static class FixedViewportCreator<TState>
    {
        public static Func<TState, TvPoint, IViewport> Return(IViewport viewport) => (st, pt) => viewport;
        public static Func<TState, TvPoint, IViewport> NullViewport() => (st, pt) => null;
    }
}
