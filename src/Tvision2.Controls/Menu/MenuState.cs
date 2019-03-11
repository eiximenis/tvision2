using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.Menu
{
    public class MenuState : IDirtyObject
    {
        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;

        private readonly List<MenuEntry> _options;

        public IEnumerable<MenuEntry> Options => _options;

        public MenuState(IEnumerable<string> options)
        {
            _options = options.Select(option => new MenuEntry(option)).ToList();
        }

        public int SelectedIndex = 1;
    }
}