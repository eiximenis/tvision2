using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.Menu
{
    public class MenuBarState : IDirtyObject
    {
        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;

        private readonly List<MenuBarEntry> _options;

        public IEnumerable<MenuBarEntry> Options => _options;

        public MenuBarState(IEnumerable<string> options)
        {
            _options = options.Select(option => new MenuBarEntry(option)).ToList();
        }

        public int SelectedIndex = 1;
    }
}