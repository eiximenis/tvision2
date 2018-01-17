using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public struct TvPoint
    {
        public int Left { get; }
        public int Top { get; }

        public TvPoint(int top, int left)
        {
            Left = left;
            Top = top;
        }
    }
}
