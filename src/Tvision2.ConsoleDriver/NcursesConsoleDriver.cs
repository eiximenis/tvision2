using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tvision2.Engine.Console;
using Tvision2.Events;
using Tvision2.Events.NCurses;
using Unix.Terminal;
using static Unix.Terminal.Curses;

namespace Tvision2.ConsoleDriver
{
    public class NcursesConsoleDriver : IConsoleDriver
    {
        private readonly ConsoleDriverOptions _options;

        private const int EscKey = 27;

        public NcursesConsoleDriver(ConsoleDriverOptions options)
        {
            _options = options;
        }

        public void Init()
        {
            Curses.initscr();
            Curses.raw();
            Curses.noecho();
            Curses.nonl();
            var stdscr = Window.Standard;
            Curses.nodelay(stdscr.Handle, bf: true);
            Curses.keypad(stdscr.Handle, bf: true);
        }

        public TvConsoleEvents ReadEvents()
        {
            var code = Curses.get_wch(out int wch);
            TvConsoleEvents events = null;

            if (code == Curses.OK)
            {
                events = new TvConsoleEvents();
                var alt = false;
                if (wch == EscKey) // Esc key read, we treat as Alt
                {
                    alt = true;
                    Curses.timeout(100);
                    var code2 = Curses.get_wch(out wch);
                    if (code2 == Curses.ERR)
                    {
                        wch = EscKey;
                        alt = false;
                    }
                }

                events.Add(new NCursesConsoleKeyboardEvent(wch, alt: alt, isDown: true));
                events.Add(new NCursesConsoleKeyboardEvent(wch, alt: alt, isDown: false));

                return events;
            }

            else if (code == Curses.KEY_CODE_YES)
            {
                events = new TvConsoleEvents();
                if (wch == Curses.KeyMouse)
                {
                    Curses.getmouse(out Curses.MouseEvent ev);
                    return TvConsoleEvents.Empty;
                }

                else
                {
                    events.Add(new NCursesConsoleKeyboardEvent(wch, alt: false, isDown: true));
                    events.Add(new NCursesConsoleKeyboardEvent(wch, alt: false, isDown: false));
                    return events;
                }

                return events;
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

        public void SetCursorAt(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public (int rows, int cols) GetConsoleWindowSize()
        {
            return (Console.WindowHeight, Console.WindowWidth);
        }
    }
}