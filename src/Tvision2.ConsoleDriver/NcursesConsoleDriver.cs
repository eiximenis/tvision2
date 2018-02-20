using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tvision2.Events;
using Unix.Terminal;
using static Unix.Terminal.Curses;

namespace Tvision2.ConsoleDriver
{
    public class NcursesConsoleDriver : IConsoleDriver
    {
        private Curses.Window _console;
        public void Init() {
            _console = Curses.initscr();
            var w = Window.Standard;
            Curses.nodelay(w.Handle, bf: true);
        }

        public TvConsoleEvents ReadEvents()
        {
            var code = Curses.get_wch(out int wch);

            if (code == Curses.OK) {
                var i =0;
            }

            if (code == Curses.KEY_CODE_YES)
            {
                if (wch == Curses.KeyMouse)
                {
                    
                    Curses.getmouse(out Curses.MouseEvent ev);
                    return TvConsoleEvents.Empty;
                }
                return TvConsoleEvents.Empty;
            }

            return TvConsoleEvents.Empty;
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
