using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.List
{
    public class ListState<T> : IDirtyObject
    {
        private readonly List<T> _values;
        private int _selectedIndex = 0;

        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;

        public IEnumerable<T> Values => _values;
        public ListStateView<T> ItemsView { get; }


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

        public bool CanScroll => SelectedIndex < Count - 1;

        public bool NeedsToScroll => SelectedIndex == ItemsView.To;

        public void ScrollDown(int lines)
        {
            ItemsView.ScrollDown(lines);
            IsDirty = true;
        }


        public ListState(IEnumerable<T> values, Func<T, TvListItem> converter)
        {
            _values = values.ToList();
            _selectedIndex = 0;
            ItemsView = new ListStateView<T>(this, converter);
        }

        public void Clear()
        {
            _values.Clear();
            ItemsView.Reload();
            IsDirty = true;
        }

        public void AddRange(IEnumerable<T> items)
        {
            _values.AddRange(items);
            ItemsView.Reload();
            IsDirty = true;
        }
    }

}