using System;

namespace Tvision2.Core.Colors
{

    public struct CharacterAttribute : IEquatable<CharacterAttribute>
    {
        
        public readonly CharacterAttributeModifiers Modifiers;
        public readonly ulong ColorIdx;

        public bool Equals(CharacterAttribute other)
        {
            return Modifiers == other.Modifiers && ColorIdx == other.ColorIdx;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)(((int) Modifiers * 397) ^ (long)ColorIdx);
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

        public CharacterAttribute(int colorIdx, CharacterAttributeModifiers modifiers = CharacterAttributeModifiers.Normal)
        {
            ColorIdx = (ulong)colorIdx;
            Modifiers = modifiers;
        }
        public CharacterAttribute(ulong colorIdx, CharacterAttributeModifiers modifiers = CharacterAttributeModifiers.Normal)
        {
            ColorIdx = colorIdx;
            Modifiers = modifiers;
        }
    }
}