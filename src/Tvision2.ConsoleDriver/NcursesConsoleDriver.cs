using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public class NcursesConsoleDriver : IConsoleDriver
    {
        public TvConsoleEvents ReadEvents()
        {
            throw new NotImplementedException();
        }

        public void WriteCharacterAt(int x, int y, char character)
        {
            throw new NotImplementedException();
        }

        public void WriteCharacterAt(int x, int y, char character, ConsoleColor foreColor, ConsoleColor backColor)
        {
            throw new NotImplementedException();
        }
    }
}
