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
            Console.WriteLine("nodelay");
            var w = Window.Standard;
            Curses.nodelay(ref w, bf: true);
            Console.WriteLine("nodelay done");
        }

        public TvConsoleEvents ReadEvents()
        {
            var code = Curses.get_wch(out int wch);
            Console.WriteLine("-->" + code + " -->" + wch);
            if (code == Curses.KEY_CODE_YES)
            {
                if (wch == Curses.KeyMouse)
                {
                    
                    Curses.getmouse(out Curses.MouseEvent ev);
                    Debug.WriteLine("GETMOUSE!!!!");
                    return TvConsoleEvents.Empty;
                }
                Debug.WriteLine("GETKEY!!!!!!");
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
