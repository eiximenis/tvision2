using System;

namespace Tvision2.Controls.Menu
{


    public interface ITvMenuBarOptionsBuilder
    {
        ITvMenuBarOptionsBuilder ItemsSpacedBy(int charcount);
        ITvMenuBarOptionsBuilder UseHotKey(ConsoleKey key);
        ITvMenuBarOptionsBuilder UseSelectKey(ConsoleKey key);
    }

    public interface ITvMenuBarOptions
    {

        public int SpaceBetweenItems { get; }
        public ConsoleKey Hotkey { get; }
        public ConsoleKey SelectKey { get; }
    }

    public class TvMenuBarOptions : ITvMenuBarOptions, ITvMenuBarOptionsBuilder
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

        ITvMenuBarOptionsBuilder ITvMenuBarOptionsBuilder.ItemsSpacedBy(int charcount)
        {
            SpaceBetweenItems = charcount;
            return this;
        }

        ITvMenuBarOptionsBuilder ITvMenuBarOptionsBuilder.UseHotKey(ConsoleKey key)
        {
            Hotkey = key;
            return this;
        }
        ITvMenuBarOptionsBuilder ITvMenuBarOptionsBuilder.UseSelectKey(ConsoleKey key)
        {
            SelectKey= key;
            return this;
        }
    }
}