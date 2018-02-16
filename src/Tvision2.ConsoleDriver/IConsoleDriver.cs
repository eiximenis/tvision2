using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public interface IConsoleDriver
    {
        void WriteCharacterAt(int x, int y, char character);
        void WriteCharacterAt(int x, int y, char character, ConsoleColor foreColor, ConsoleColor backColor);
        TvConsoleEvents ReadEvents();
    }
}
