﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public readonly struct TvBounds
    {
        public static TvBounds Empty { get; } = new TvBounds(0, 0);
        public int Rows { get; }
        public int Cols { get; }

        public void Deconstruct(out int rows, out int cols)
        {
            rows = Rows;
            cols = Cols;
        }

        private TvBounds(int rows, int cols)
        {
            Rows = rows >= 0 ? rows : 0;
            Cols = cols >= 0 ? cols : 0;
        }

        public int Length() => Rows * Cols;

        public static TvBounds FromRowsAndCols(int rows, int cols) => new TvBounds(rows, cols);

        public TvBounds SingleRow() => new TvBounds(Rows > 0 ? 1 : 0, Cols);
        public TvBounds SingleColumn() => new TvBounds(Rows, Cols > 0 ? 1 : 0);

        public bool HigherThan(TvBounds other) => Rows > other.Rows;
        public bool HigherOrEqualThan(TvBounds other) => Rows >= other.Rows;

        public bool WiderThan(TvBounds other) => Cols > other.Cols;
        public bool WiderOrEqualThan(TvBounds other) => Cols >= other.Cols;

        public override bool Equals(object obj)
        {
            if (obj is TvBounds other)
            {
                return other.Cols == Cols && other.Rows == Rows;
            }
            return base.Equals(obj);
        }

        public TvBounds Grow(int rowsToGrow, int colsToGrow) => new TvBounds(Rows + rowsToGrow, Cols + colsToGrow);

        public TvBounds Reduce(TvBounds reduction) => new TvBounds(Rows - reduction.Rows, Cols - reduction.Cols);

        public override int GetHashCode()
        {
            return (Cols << 16) & (Rows);
        }

        public static bool operator ==(TvBounds first, TvBounds second) => first.Equals(second);
        public static bool operator !=(TvBounds first, TvBounds second) => !first.Equals(second);

        public static TvBounds Min(TvBounds first, TvBounds second)
        {
            return TvBounds.FromRowsAndCols(first.Rows <= second.Rows ? first.Rows : second.Rows, first.Cols <= second.Cols ? first.Cols : second.Cols);
        }

        public override string ToString() => $"({Rows}r x {Cols}c)";
    }
}
