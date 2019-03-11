namespace Tvision2.Controls.Menu
{
    public class MenuEntry
    {
        public string Text { get; }
        public char Shortcut { get; }
        public int ShortcutPos { get; }
        public MenuEntry(string text)
        {

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
