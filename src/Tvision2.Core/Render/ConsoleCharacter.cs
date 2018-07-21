using System;

namespace Tvision2.Core.Render
{
    public class ConsoleCharacter
    {
        public uint Value { get; set; }


        public char Character
        {
            get => (char)(Value & 0x0000FFFF);
            set => Value |= value;
        }
        public ConsoleColor Foreground
        {
            get => (ConsoleColor)((Value & 0x00FF0000) >> 16);
            set => Value |= ((uint)value << 16);
        }
        public ConsoleColor Background
        {
            get => (ConsoleColor)((Value & 0xFF000000) >> 24);
            set => Value |= ((uint)value << 24);
        }

        public int ZIndex { get; set; }

        public bool Equals(ConsoleCharacter other)
        {
            return Value == other.Value && ZIndex == other.ZIndex;
        }

        public bool Equals(char otherChar, ConsoleColor otherForeColor, ConsoleColor otherBackColor, int otherZIndex)
        {
            var other = otherChar | ((uint)otherForeColor << 16) | ((uint)otherBackColor << 24);

            return Value == other &&
                ZIndex == otherZIndex;

        }

        public static uint ValueFrom(char @char, ConsoleColor foreColor, ConsoleColor backColor)
        {
            return @char | ((uint)foreColor << 16) | ((uint)backColor << 24);
        }
    }
}
