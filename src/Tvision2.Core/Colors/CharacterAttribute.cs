using System;

namespace Tvision2.Core.Colors
{
    public struct CharacterAttribute : IEquatable<CharacterAttribute>
    {
        
        public CharacterAttributeModifiers Modifiers;
        public int ColorIdx;

        public bool Equals(CharacterAttribute other)
        {
            return Modifiers == other.Modifiers && ColorIdx == other.ColorIdx;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Modifiers * 397) ^ ColorIdx;
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
            ColorIdx = colorIdx;
            Modifiers = modifiers;
        }
              
    }
}