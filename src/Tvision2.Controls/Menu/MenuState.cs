using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.Menu
{
    public class MenuState : IDirtyObject
    {
        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;
        private int _selectedIndex;

        private readonly List<MenuEntry> _options;

        public MenuState()
        {
            _selectedIndex = 0;
            _options = new List<MenuEntry>();
        }

        public MenuState(IEnumerable<string> options)
        {
            _selectedIndex = 0;
            _options = options.Select(option => new MenuEntry(option)).ToList();
        }

        public IEnumerable<MenuEntry> Entries => _options;

        public MenuEntry this[string value] => _options.FirstOrDefault(o => o.Text == value);

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value < 0) _selectedIndex = _options.Count - 1;
                if (value >= _options.Count) _selectedIndex = 0;
                else _selectedIndex = value;
            }
        }

        public MenuEntry SelectedEntry => _options[SelectedIndex];
    }
}