using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class VirtualConsoleUpdate
    {
        public ConsoleCharacter NewCharacter { get; }
        public int Row { get;}
        public int Column { get; }

        public VirtualConsoleUpdate(ConsoleCharacter @new, int row, int column)
        {
            NewCharacter = @new;
            Row = row;
            Column = column;
        }
    }
}
