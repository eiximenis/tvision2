using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public struct TvPoint
    {
        public static TvPoint Zero { get; } = new TvPoint(0, 0);
        public int Left { get; } 
        public int Top { get; }

        public TvPoint(int top, int left)
        {
            Left = left;
            Top = top;
        }

        public static TvPoint operator+ (TvPoint first, TvPoint second)
        {
            return new TvPoint(first.Top + second.Top, first.Left + second.Left);
        }
    }
}
