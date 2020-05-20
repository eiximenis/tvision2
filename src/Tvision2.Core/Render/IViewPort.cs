using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public enum FlowModel
    {
        None,               // Content outside Bounds is clipped
        LineBreak           // Content outside Bounds breaks line
    }

    public interface IViewport : IEquatable<IViewport>
    {
        TvPoint Position { get; }
        Layer ZIndex { get; }
        TvBounds Bounds { get; }

        FlowModel Flow { get; }

        IViewport ResizeTo(int cols, int rows);
        IViewport Grow(int ncols, int nrows);
        IViewport MoveTo(TvPoint newPos);
        IViewport Translate(TvPoint translation);
        IViewport Clone();
    }
}
