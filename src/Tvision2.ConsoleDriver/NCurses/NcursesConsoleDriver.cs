using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using Tvision2.ConsoleDriver.NCurses;
using Tvision2.Core.Colors;
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
        private readonly NcursesColorManager _colorDriver;

        private const int EscKey = 27;

        public NcursesConsoleDriver(ConsoleDriverOptions options, NcursesColorManager colorDriver)
        {
            _options = options;
            _colorDriver = colorDriver;
        }

        public void Init()
        {
            Curses.setlocale(6, "");
            Curses.initscr();
            _colorDriver.Init();
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

        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attributes)
        {

            _colorDriver.SetAttributes(attributes);
            //_colorDriver.SetColor(pairIdx, CharacterAttributes.Italic | CharacterAttributes.Blink);
            //Console.SetCursorPosition(x, y);

            //var wchar = new CcharT(pairIdx << 8, character);
            // Curses.mvadd_wch(y, x, ref wchar);
            //Curses.addch(character);
            Curses.move(y, x);
            Curses.addch(character);
        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            // TODO: Temporal hack for compilation. Needs to be refactored to make a single native call
            for (var rep = 0; rep < count; rep++)
            {
                WriteCharacterAt(x + rep, y, character, attribute);
            }
        }

        public void SetCursorAt(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public (int rows, int cols) GetConsoleWindowSize()
        {
            return (Console.WindowHeight, Console.WindowWidth);
        }

        public void SetCursorVisibility(bool isVisible)
        {
        }
    }
}