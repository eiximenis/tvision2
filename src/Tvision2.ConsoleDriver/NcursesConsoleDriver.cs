using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tvision2.Events;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver
{
    public class NcursesConsoleDriver : IConsoleDriver
    {
        public TvConsoleEvents ReadEvents()
        {
            var code = Curses.get_wch(out int wch);
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
