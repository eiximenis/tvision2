using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public interface IViewport : IEquatable<IViewport>
    {
        TvPoint Position { get; }
        int ZIndex { get; }
        int Columns { get; }
        int Rows { get; }

        bool Intersects(IViewport another);

        IViewport ResizeTo(int cols, int rows);
        IViewport Grow(int ncols, int nrows);
        IViewport MoveTo(TvPoint newPos);
        IViewport Translate(TvPoint translation);
        IViewport Clone();
        IViewport Top();

    }
}
