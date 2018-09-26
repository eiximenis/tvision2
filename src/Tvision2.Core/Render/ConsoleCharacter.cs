using System;

namespace Tvision2.Core.Render
{
    public class ConsoleCharacter
    {

        public char Character { get; set; }
        public int PairIndex { get; set; }        
        public int ZIndex { get; set; }

        public bool Equals(ConsoleCharacter other)
        {
            return Character == other.Character && PairIndex == other.PairIndex && ZIndex == other.ZIndex;
        }

        public bool Equals(char otherChar, int otherPairIdx, int otherZIndex)
        {
            return Character == otherChar && PairIndex == otherPairIdx && ZIndex == otherZIndex;
        }

    }
}
