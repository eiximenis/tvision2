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
       

        public ConsoleCharacterComparison CompareContents(char otherChar, CharacterAttribute otherAttr)
        {
            if (Character == otherChar)
            {
                if (Attributes != otherAttr)
                {
                    return ConsoleCharacterComparison.DifferOnlyAttributes;
                }
                return ConsoleCharacterComparison.Equals;
            }
            else
            {
                if (Attributes != otherAttr)
                {
                    return ConsoleCharacterComparison.DifferOnEverything;
                }
                return ConsoleCharacterComparison.DifferOnlyChar;
            }
        }
    }
}
