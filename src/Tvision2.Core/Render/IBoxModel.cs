using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public interface IBoxModel
    {
        TvPoint Position { get; }
        int ZIndex { get; set; }
        int Columns { get; }
        int Rows { get; }
        ClippingMode Clipping { get; }

        bool Grow(int cols, int rows);

    }
}
