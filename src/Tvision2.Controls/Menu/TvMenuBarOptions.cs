using System;

namespace Tvision2.Controls.Menu
{

    public interface ITvMenuBarOptions
    {
        ITvMenuBarOptions ItemsSpacedBy(int charcount);
        ITvMenuBarOptions UseHotKey(ConsoleKey key);
        ITvMenuBarOptions UseSelectKey(ConsoleKey key);
    }

    public class TvMenuBarOptions : ITvMenuBarOptions
    {
        public int SpaceBetweenItems { get; private set; }
        public ConsoleKey Hotkey  {get; private set; }

        public ConsoleKey SelectKey { get; private set; }

        public TvMenuBarOptions()
        {
            SpaceBetweenItems = 4;
            Hotkey = ConsoleKey.NoName;
            SelectKey = ConsoleKey.Enter;
        }

        ITvMenuBarOptions ITvMenuBarOptions.ItemsSpacedBy(int charcount)
        {
            SpaceBetweenItems = charcount;
            return this;
        }

        ITvMenuBarOptions ITvMenuBarOptions.UseHotKey(ConsoleKey key)
        {
            Hotkey = key;
            return this;
        }
        ITvMenuBarOptions ITvMenuBarOptions.UseSelectKey(ConsoleKey key)
        {
            SelectKey= key;
            return this;
        }
    }
}