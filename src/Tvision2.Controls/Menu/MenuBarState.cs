using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.Menu
{
    public class MenuBarState : IDirtyObject
    {
        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;

        private readonly List<string> _options;

        public IEnumerable<string> Options => _options;

        public MenuBarState(IEnumerable<string> options)
        {
            _options = options.ToList();
        }
    }
}