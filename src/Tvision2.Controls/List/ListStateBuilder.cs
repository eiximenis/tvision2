using System;
using System.Collections.Generic;

namespace Tvision2.Controls.List
{

    internal class ListStateBuilder<T> : IListStateColumnsSpecifier<T>
    {

        private IEnumerable<T> _source;
        private List<TvListColumnSpec<T>> _columns;

        public ListStateBuilder(IEnumerable<T> source)
        {
            _source = source;
            _columns = new List<TvListColumnSpec<T>>();
        }

        public IListStateColumnsSpecifier<T> AddColumn(Func<T, string> columnTransformer)
        {
            _columns.Add(new TvListColumnSpec<T>
            {
                Transformer = columnTransformer,
                Width = 0
            });

            return this;
        }

        public IListStateColumnsSpecifier<T> AddFixedColumn(Func<T, string> columnTransformer, int width)
        {
            _columns.Add(new TvListColumnSpec<T>
            {
                Transformer = columnTransformer,
                Width = width
            });

            return this;
        }

        public ListState<T> Build()
        {
            return new ListState<T>(_source, _columns.ToArray());
        }
    }
}