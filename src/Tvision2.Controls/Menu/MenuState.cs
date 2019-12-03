using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.Menu
{
    public class MenuState : IDirtyObject
    {
        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;

        private readonly List<MenuEntry> _options;

        public MenuState(IEnumerable<string> options)
        {
            _options = options.Select(option => new MenuEntry(option)).ToList();
        }

        public IEnumerable<MenuEntry> Entries => _options;

        public MenuEntry this[string value] => _options.FirstOrDefault(o => o.Text == value);

        public int SelectedIndex = 0;

        public MenuEntry SelectedEntry => _options[SelectedIndex];
    }
}