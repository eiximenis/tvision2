using System;
using Tvision2.Core.Colors;
using Attribute = System.Attribute;

namespace Tvision2.Core.Render
{
    public class ConsoleCharacter
    {

        public char Character { get; set; }
        public CharacterAttribute Attributes { get; set; }        
        public int ZIndex { get; set; }

        public bool Equals(ConsoleCharacter other)
        {
            return Character == other.Character && Attributes == other.Attributes && ZIndex == other.ZIndex;
        }

        public bool Equals(char otherChar, CharacterAttribute attr,  int otherZIndex)
        {
            return Character == otherChar  && Attributes == attr && ZIndex == otherZIndex;
        }

    }
}
