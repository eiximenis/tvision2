using System;

namespace Tvision2.Core.Colors
{

    public struct CharacterAttribute : IEquatable<CharacterAttribute>
    {
        
        public readonly CharacterAttributeModifiers Modifiers;
        public readonly TvColor Fore;
        public readonly TvColor Back;

        public bool Equals(CharacterAttribute other)
        {
            return Modifiers == other.Modifiers && Back == other.Back && Fore == other.Fore;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)(((int) Modifiers * 397) ^ (long)(Fore.Value << 31 | Back.Value));
            }
        }

        public static bool operator ==(CharacterAttribute left, CharacterAttribute right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CharacterAttribute left, CharacterAttribute right)
        {
            return !left.Equals(right);
        }
        

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CharacterAttribute other && Equals(other);
        }

        public CharacterAttribute(TvColorPair colors, CharacterAttributeModifiers modifiers = CharacterAttributeModifiers.Normal)
        {
            Back = colors.Background;
            Fore = colors.ForeGround;
            Modifiers = modifiers;
        }
    }
}