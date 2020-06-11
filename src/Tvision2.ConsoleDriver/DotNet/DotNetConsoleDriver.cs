using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.Colors;
using Tvision2.ConsoleDriver.DotNet;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    class DotNetConsoleDriver : IConsoleDriver
    {
        private readonly ConsoleDriverOptions _options;
        private readonly DotNetColorManager _colorManager;

        public IColorManager ColorManager => _colorManager;
        public TvBounds ConsoleBounds { get; private set; }

        public DotNetConsoleDriver(ConsoleDriverOptions options, DotNetColorManager colorManager)
        {
            _options = options;
            _colorManager = colorManager;
        }

        public void Init()
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
        }
        public ITvConsoleEvents ReadEvents()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                var events = new TvConsoleEvents();
                events.Add(new DotNetConsoleKeyboardEvent(key));
                return events;
            }
            else
            {
                return TvConsoleEvents.Empty;
            }
        }


        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute)
        {
            var (fg, bg) = _colorManager.AttributeToDotNetColors(attribute);
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.SetCursorPosition(x, y);
            Console.Write(character);
        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            for (var rep = 0; rep < count; rep++)
            {
                WriteCharacterAt(x + rep, y, character, attribute);
            }
        }


        public void SetCursorAt(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public void SetCursorVisibility(bool isVisible)
        {
            Console.CursorVisible = isVisible;
        }

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
            windowEvent.Update(ConsoleBounds.Cols, ConsoleBounds.Rows); 
        }
    }
}
