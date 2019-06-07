using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Events
{
    public class TvWindowEvent
    {
        public int NewColumns { get; private set; }
        public int NewRows { get; private set; }


        public TvWindowEvent(int cols, int rows)
        {
            NewColumns = cols;
            NewRows = rows;
        }

        public void Update(int newCols, int newRows)
        {
            NewColumns = newCols;
            NewRows = newRows;
        }
    }
}
