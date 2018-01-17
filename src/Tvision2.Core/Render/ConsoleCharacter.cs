using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class ConsoleCharacter
    {
        public char Character { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public int ZIndex { get; set; }

        public bool Equals(ConsoleCharacter other)
        {
            return Character == other.Character &&
                Foreground == other.Foreground &&
                Background == other.Background;
        }
    }
}
