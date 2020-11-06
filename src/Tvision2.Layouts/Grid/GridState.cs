using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Layouts.Grid
{
    public class GridState
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public GridState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }

        public static GridState FromRowsAndColumns(int rows, int columns) => new GridState(rows, columns);
    }
}
