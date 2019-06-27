using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Win32
{
    class Win32AnsiSequencesManager : IColorManager
    {

        private const string SGR_RGB_FORE = "\x1b[38;2;{0};{1};{2}m";
        private const string SGR_RGB_BACK = "\x1b[48;2;{0};{1};{2}m";
        private const string SGR = "\x1b[{0}m";
        private const string CUP = "\x1b[{0};{1}H";
        private const ulong BACKCOL_MASK = 0xFFFFFFFF00000000;
        public int MaxColors => -1;

        public CharacterAttribute DefaultAttribute { get; }

        public Win32AnsiSequencesManager()
        {
            DefaultAttribute = BuildAttributeFor(TvColor.White, TvColor.Black, CharacterAttributeModifiers.Normal);
        }

        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            return CharacterAttributeExtensions.EncodeColorsInAttribute(fore, back, attrs);
        }

        public string GetAttributeSequence(CharacterAttribute attributes)
        {
            var (fore, back) = attributes.DecodeColorsFromAttribute();
            var foreString = (string)null;
            if (fore.IsRgb)
            {
                var (fr, fg, fb) = fore.Rgb;
                foreString = string.Format(SGR_RGB_FORE, fr.ToString(), fg.ToString(), fb.ToString());
            }
            else
            {
                foreString = string.Format(SGR, (fore.Value + 30).ToString());
            }
            var backString = (string)null;
            if (back.IsRgb)
            {
                var (br, bg, bb) = back.Rgb;
                backString = string.Format(SGR_RGB_BACK, br.ToString(), bg.ToString(), bb.ToString());
            }
            else
            {
                backString = string.Format(SGR, (back.Value + 40).ToString());
            }

            return foreString + backString;
        }

        public string GetCursorSequence(int x, int y)
        {
            return string.Format(CUP, y + 1, x + 1);
        }
    }
}

