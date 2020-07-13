using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorTerm
{
    public struct BtermColor
    {
        public byte r { get; }
        public byte g { get; }
        public byte b { get; }

        public BtermColor(byte red, byte green, byte blue)
        {
            r = red;
            g = green;
            b = blue;
        }
    }

    public struct BtermCharacter
    {
        public int CodePoint {get; set;}

        public BtermColor Fore { get; set; }

        public BtermColor Back { get; set; }
        public BtermCharacter(char codepoint, in BtermColor fore, in BtermColor back)
        {
            CodePoint = (int)codepoint;
            Fore = fore;
            Back = back;
        }

        public static BtermCharacter FromCodePoint(char cp) => new BtermCharacter(cp, new BtermColor(0xa0, 0xa0, 0xa0), new BtermColor(0, 0, 0));
    }
}
