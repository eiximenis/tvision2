using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class VirtualConsoleUpdate
    {
        public ConsoleCharacter Old { get; }
        public ConsoleCharacter New { get; }
        public int Row { get;}
        public int Column { get; }

        public VirtualConsoleUpdate(ConsoleCharacter old, ConsoleCharacter @new, int row, int column)
        {
            Old = old;
            New = @new;
            Row = row;
            Column = column;
        }
    }
}
