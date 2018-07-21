using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.List
{
    public class ListState : IDirtyObject
    {
        private readonly List<string> _values;
        private int _selectedIndex = 0;
        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;

        public IEnumerable<string> Values => _values;
        public ListStateView ItemsView { get; }


        public string this[int idx] => _values[idx];
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


        public ListState(IEnumerable<string> values)
        {
            _values = values.ToList();
            _selectedIndex = 0;
            ItemsView = new ListStateView(this);
        }

        public void Clear()
        {
            _values.Clear();
            ItemsView.Reload();
            IsDirty = true;
        }

        public void AddRange(IEnumerable<string> items)
        {
            _values.AddRange(items);
            ItemsView.Reload();
            IsDirty = true;
        }

    }

}