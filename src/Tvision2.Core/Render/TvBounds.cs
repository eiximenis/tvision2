using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public readonly struct TvBounds
    {
        public static TvBounds Empty { get; } = new TvBounds(0, 0);
        public int Rows { get; }
        public int Cols { get; }

        private TvBounds(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }

        public static TvBounds FromRowsAndCols(int rows, int cols) => new TvBounds(rows, cols);

        public TvBounds SingleRow() => new TvBounds(1, Cols);

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
        
        public override int GetHashCode()
        {
            return (Cols << 16) & (Rows);
        }

        public static bool operator ==(TvBounds first, TvBounds second) => first.Equals(second);
        public static bool operator !=(TvBounds first, TvBounds second) => !first.Equals(second);
    }
}
