using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.List
{
    public interface IListStateColumnsSpecifier<T>
    {
        IListStateColumnsSpecifier<T> AddFixedColumn(Func<T, string> columnTransformer, int width);
        IListStateColumnsSpecifier<T> AddColumn(Func<T, string> columnTransformer);
        ListState<T> Build();
    }
}
