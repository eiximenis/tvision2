using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver.DotNet
{
    class DotNetConsoleKeyboardEvent : TvConsoleKeyboardEvent
    {

        private readonly ConsoleKeyInfo _consoleKey;

        public DotNetConsoleKeyboardEvent(ConsoleKeyInfo consoleKey) => _consoleKey = consoleKey;

        public override ConsoleKeyInfo AsConsoleKeyInfo() => _consoleKey;
    }
}
