using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.DotNet;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public class NetConsoleDriver : IConsoleDriver
    {

        public void Init() {}
        public TvConsoleEvents ReadEvents()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                var events = new TvConsoleEvents();
                events.Add(new DotNetConsoleKeyboardEvent(key, isDown: true));
                events.Add(new DotNetConsoleKeyboardEvent(key, isDown: false));
                return events;
            }
            else
            {
                return TvConsoleEvents.Empty;
            }
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
