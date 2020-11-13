using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;
using Tvision2.ConsoleDriver.Common;
using Tvision2.ConsoleDriver.Linux.NCurses;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;
using Tvision2.Events.NCurses;
using Unix.Terminal;
using static Unix.Terminal.Curses;

namespace Tvision2.ConsoleDriver
{
    public class NcursesConsoleDriver : IConsoleDriver
    {
        private readonly LinuxConsoleDriverOptions _options;
        private readonly NcursesColorManager _colorDriver;

        private const int EscKey = 27;

        public IColorManager ColorManager => _colorDriver;
        public TvBounds ConsoleBounds { get; private set; }
        public TvColor DefaultBackground { get => _options.DefaultBackColor; }
        public NcursesConsoleDriver(LinuxConsoleDriverOptions options, NcursesColorManager colorDriver)
        {
            _options = options;
            _colorDriver = colorDriver;
        }

        public void Init()
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
            
            Curses.setlocale(6, "");
            Curses.initscr();
            _colorDriver.Init();
            Curses.raw();
            Curses.noecho();
            Curses.nonl();
            var stdscr = Window.Standard;
            Curses.nodelay(stdscr.Handle, bf: true);
            Curses.keypad(stdscr.Handle, bf: true);
            if (_options.MouseStatusDesired == MouseStatus.Enabled)
            {
                TryEnableMouse();
            }            
        }

        private void TryEnableMouse()
        {
            Curses.mousemask(Event.AllEvents, out var oldEvents);
        }

        public void End()
        {
            Curses.endwin();
        }

        public ITvConsoleEvents ReadEvents()
        {
            var code = Curses.get_wch(out int wch);
            TvConsoleEvents events = null;

            switch (code)
            {
                case Curses.OK when wch == 3:
                {
                    events = new TvConsoleEvents();
                    events.Signal(TvConsoleSignal.Sigint);
                    break;
                }
                case Curses.OK:
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
                    events.Add(new NCursesConsoleKeyboardEvent(wch, alt: alt));
                    break;
                }
                case Curses.KEY_CODE_YES when wch == Curses.KeyMouse:
                {
                    if (Curses.getmouse(out Curses.MouseEvent ev) == Curses.OK)
                    {
                        events = new TvConsoleEvents();
                        events.Add(CursesMouseEventToTvisionMouseEvent(in ev));
                    }
                    
                    break;
                }
                case Curses.KEY_CODE_YES:
                {
                    events = new TvConsoleEvents();
                    events.Add(new NCursesConsoleKeyboardEvent(wch, alt: false));
                    break;
                }
            }
            return events ??  TvConsoleEvents.Empty;
        }

        private TvConsoleMouseEvent CursesMouseEventToTvisionMouseEvent(in MouseEvent ev)
        {
            var states = (TvMouseButtonStates) ev.ButtonState;                // TvMouseButtonStates have same values as Curses.States
            return new TvConsoleMouseEvent(ev.X, ev.Y, states);
        }


        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attributes)
        {
            _colorDriver.SetAttributes(attributes);
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

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
            windowEvent.Update(ConsoleBounds.Cols, ConsoleBounds.Rows);
        }
    }
}