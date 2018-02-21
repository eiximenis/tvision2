using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tvision2.Events;
using Tvision2.Events.NCurses;
using Unix.Terminal;
using static Unix.Terminal.Curses;

namespace Tvision2.ConsoleDriver
{
    public class NcursesConsoleDriver : IConsoleDriver
    {
        private Curses.Window _console;
        public void Init() 
        {
            _console = Curses.initscr();
            Curses.raw();
            Curses.noecho();
            Curses.nonl();
            var w = Window.Standard;
            Curses.nodelay(w.Handle, bf: true);
            Curses.keypad(w.Handle, bf: true);
            Curses.get_wch(out int wch);
        }

        public TvConsoleEvents ReadEvents()
        {
            var code = Curses.get_wch(out int wch);
            TvConsoleEvents events = null;

            if (code == Curses.OK) 
            {
                events = new TvConsoleEvents();
                var alt = false;
                if (code == 27)     // Alt read, next char has the key
                {
                    Curses.get_wch(out wch);
                    alt = true;
                }
                 events.Add(new NCursesConsoleKeyboardEvent(wch, alt: alt, isDown: true));
                 events.Add(new NCursesConsoleKeyboardEvent(wch, alt: alt, isDown: false));
            }

            if (code == Curses.KEY_CODE_YES)
            {
                events = new TvConsoleEvents();
                if (wch == Curses.KeyMouse)
                {
                    
                    Curses.getmouse(out Curses.MouseEvent ev);
                    return TvConsoleEvents.Empty;
                }
                return TvConsoleEvents.Empty;
            }

            return events ?? TvConsoleEvents.Empty;
        }

        public void WriteCharacterAt(int x, int y, char character)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }

        public void WriteCharacterAt(int x, int y, char character, ConsoleColor foreColor, ConsoleColor backColor)
        {
            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }
    }
}
