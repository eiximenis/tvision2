using System;

namespace Tvision2.Core.Colors
{
    [Flags]
    public enum CharacterAttributeModifiers
    {
        Normal = 0,
        Standout = 1 << 8,
        Underline = 1 << 9,
        Reverse = 1 << 10,
        Blink = 1 << 11,
        Dim = 1 << 12,
        Bold = 1 << 13,
        Invis = 1 << 15,
        Protect = 1 << 16,
        Italic = 1 << 23,
        BackgroundBold = 1 << 24
    }
}