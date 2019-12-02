using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.Menu
{
    public class MenuEntry
    {
        private  readonly List<MenuEntry> _options;

        public string Text { get; }
        public char Shortcut { get; }
        public int ShortcutPos { get; }

        public IEnumerable<MenuEntry> ChildEntries => _options;

        public MenuEntry this[string value] => _options.FirstOrDefault(o => o.Text == value);

        public MenuEntry AddChild(string childText)
        {
            var child = new MenuEntry(childText);
            _options.Add(child);
            return child;
        }

        public MenuEntry(string text)
        {
            _options = new List<MenuEntry>();
            (ShortcutPos, Shortcut) = FindShortcutByText(text);
            Text = text.Replace("_", "");
        }

        private (int, char) FindShortcutByText(string text)
        {
            var idx = text.IndexOf('_');
            if (idx != -1)
            {
                return (idx, text[idx + 1]);
            }
            return (-1, '\0');
        }
    }
}
