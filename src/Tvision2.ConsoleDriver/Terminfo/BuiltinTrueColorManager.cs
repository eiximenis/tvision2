using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Logging.Internal;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public class BuiltinTrueColorManager : ITerminfoColorManager
    {

        private const string setafFormat = "\x1b[38;2;{0};{1};{2}m";
        private const string setabFormat = "\x1b[48;2;{0};{1};{2}m";

        private const int MAXPALETTESIZE = 256; 

        public IPalette Palette { get; }
       
        
        public BuiltinTrueColorManager(PaletteOptions options)
        {
            Palette = new DirectPalette(MAXPALETTESIZE, options);
        }

        public void Init()
        {
        }
   
        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            return new CharacterAttribute(new TvColorPair(fore, back), attrs);
        }
        
        


        private (TvColor fore, TvColor back) DecodeColorsFromAttribute(CharacterAttribute attr)
        {
            return (attr.Fore, attr.Back);
        }
        

        public CharacterAttribute DefaultAttribute { get; }


        public void SetAttributes(CharacterAttribute attributes)
        {
            var (fore, back) = DecodeColorsFromAttribute(attributes);

            if (!fore.IsRgb)
            {
                fore = GetRgbColorFromPalette(fore);
            }

            if (!back.IsRgb)
            {
                back = GetRgbColorFromPalette(back);
            }
            
            var (fr, fg, fb) = fore.Rgb;
            var foreString = string.Format(setafFormat, fr.ToString(), fg.ToString(), fb.ToString());
            TerminfoBindings.putp(foreString);
            var (br, bg, bb) = back.Rgb;
            var backString = string.Format(setabFormat, br.ToString(), bg.ToString(), bb.ToString());
            TerminfoBindings.putp(backString);
        }

        private TvColor GetRgbColorFromPalette(TvColor color)
        {
            var idx = color.PaletteIndex;
            return Palette[idx];
        }
    }
}