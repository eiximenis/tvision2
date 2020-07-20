using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Unix.Terminal;

namespace Tvision2.Events.NCurses
{
    class NCursesConsoleKeyboardEvent : TvConsoleKeyboardEvent
    {
        private readonly int _wch;
        private readonly bool _alt;

        private ConsoleKey NCursesToConsoleKey()
        {
            switch (_wch)
            {
                case Curses.KeyDown: return ConsoleKey.DownArrow;
                case Curses.KeyUp: return ConsoleKey.UpArrow;
                case Curses.KeyBackspace: return ConsoleKey.Backspace;
                case Curses.KeyF1: return ConsoleKey.F1;
                case Curses.KeyF2: return ConsoleKey.F2;
                case Curses.KeyF3: return ConsoleKey.F3;
                case Curses.KeyF4: return ConsoleKey.F4;
                case Curses.KeyF5: return ConsoleKey.F5;
                case Curses.KeyF6: return ConsoleKey.F6;
                case Curses.KeyF7: return ConsoleKey.F7;
                case Curses.KeyF8: return ConsoleKey.F8;
                case Curses.KeyF9: return ConsoleKey.F9;
                case Curses.KeyF10: return ConsoleKey.F10;
                case 27: return ConsoleKey.Escape;
                case 13: return ConsoleKey.Enter;
                case 9: return ConsoleKey.Tab;
 
            }

            return ConsoleKey.NoName;

        }

        public NCursesConsoleKeyboardEvent(int wch, bool alt) 
        {
            
            _wch = wch;
            _alt = alt;
        }

        public override ConsoleKeyInfo AsConsoleKeyInfo()
        {
            var ctrl = (_wch & 0x1f) != 0;            
            return new ConsoleKeyInfo((char)_wch, NCursesToConsoleKey(), false, _alt, ctrl);
        }
    }
}