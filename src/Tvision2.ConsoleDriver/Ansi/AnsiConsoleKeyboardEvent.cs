using System;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver.Ansi
{
    public class AnsiConsoleKeyboardEvent : TvConsoleKeyboardEvent
    {

        private readonly ConsoleKeyInfo _keyinfo;
        
        public AnsiConsoleKeyboardEvent(ConsoleKeyInfo info)
        {
            _keyinfo = info;
        }

        public override ConsoleKeyInfo AsConsoleKeyInfo() => _keyinfo;
    }
}