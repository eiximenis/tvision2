using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.List
{

    public static class ListState
    {
        public static ListState<T> FromEnumerable<T>(IEnumerable<T> data, params TvListColumnSpec<T>[] columns)
        {
            return new ListState<T>(data, columns);
        }
    }

    public class ListState<T> : IDirtyObject
    {
        private readonly List<T> _values;
        private int _selectedIndex = 0;

        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;

        public IEnumerable<T> Values => _values;
        public ListStateView<T> ItemsView { get; }

        internal TvListColumnSpec<T>[] Columns { get; }
        internal int ColumnsTotalFixedWidth { get; }

        private TvListItemCache<T> _cache;

        internal void SetCache(TvListItemCache<T> cache) => _cache = cache;

        public T this[int idx] => _values[idx];
        public int Count => _values.Count;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    IsDirty = true;
                }
            }
        }

        public bool CanScrollDown => SelectedIndex < Count - 1;
        public bool CanScrollUp => SelectedIndex > 0;

        public bool NeedsToScrollDown => SelectedIndex == ItemsView.To;
        public bool NeedsToScrollUp => SelectedIndex == ItemsView.From;

        public void ScrollDown(int lines)
        {
            ItemsView.ScrollDown(lines);
            IsDirty = true;
        }

        public void ScrollUp(int lines)
        {
            ItemsView.ScrollUp(lines);
            IsDirty = true;
        }

        public ListState()
        {
            _values = new List<T>();
            Columns = Array.Empty<TvListColumnSpec<T>>();
        }

        public ListState(IEnumerable<T> values, params TvListColumnSpec<T>[] columns)
        {
            _values = values.ToList();
            _selectedIndex = 0;
            ItemsView = new ListStateView<T>(this);

            if (columns.Count(c => c.Width == 0) > 1)
            {
                throw new ArgumentException("Only one column of variable (0) width is allowed.", nameof(columns));
            }

            Columns = columns;
            ColumnsTotalFixedWidth = columns.Sum(c => c.Width);
        }



        public void Clear()
        {
            _values.Clear();
            ItemsView.Reload();
            _cache.Invalidate();
            SelectedIndex = 0;
            IsDirty = true;
        }

        public void AddRange(IEnumerable<T> items)
        {
            _values.AddRange(items);
            ItemsView.Reload();
            IsDirty = true;
        }

        public static IListStateColumnsSpecifier<T> From(IEnumerable<T> source)
        {
            return new ListStateBuilder<T>(source);
        }
    }

}