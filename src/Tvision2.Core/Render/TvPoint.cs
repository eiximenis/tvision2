﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public struct TvPoint
    {
        public static TvPoint Zero { get; } = new TvPoint(0, 0);
        public int Left { get; } 
        public int Top { get; }

        public TvPoint(int left, int top)
        {
            Left = left;
            Top = top;
        }

        public static TvPoint operator+ (TvPoint first, TvPoint second)
        {
            return new TvPoint(first.Left + second.Left, first.Top + second.Top);
        }

        public static TvPoint operator -(TvPoint first, TvPoint second)
        {
            return new TvPoint(first.Left - second.Left, first.Top - second.Top);
        }

        public override bool Equals(object obj)
        {
            if (obj is TvPoint)
            {
                var other = (TvPoint)obj;
                return other.Left == Left && other.Top == Top;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (Top << 16) & (Left);
        }
    }
}
