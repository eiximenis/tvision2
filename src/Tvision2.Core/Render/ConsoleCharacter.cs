using Tvision2.Core.Colors;

namespace Tvision2.Core.Render
{
    public readonly struct ConsoleCharacter
    {

        private static ConsoleCharacter _none = new ConsoleCharacter();
        public static ConsoleCharacter None => _none;

        public char Character { get; }
        public CharacterAttribute Attributes { get; }
        public int ZIndex { get; }

        public ConsoleCharacter(char value, CharacterAttribute attribute, int zindex)
        {
            Character = value;
            Attributes = attribute;
            ZIndex = zindex;
        }

        public ConsoleCharacter(char value, CharacterAttribute attribute, Layer zindex)
        {
            Character = value;
            Attributes = attribute;
            ZIndex = (int)zindex;
        }

        public bool Equals(in ConsoleCharacter other)
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
