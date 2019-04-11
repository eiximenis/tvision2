using System;
using System.Net;
using Microsoft.Extensions.Logging.Internal;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public class BuiltinTrueColorManager : ITerminfoColorManager
    {

        private const string setafFormat = "\x1b[38;2;{0};{1};{2}m";
        private const string setabFormat = "\x1b[48;2;{0};{1};{2}m";
        
        
        public int MaxColors { get; }
        
        public BuiltinTrueColorManager()
        {
            MaxColors = 16 * 1024 * 1024;
        }

        public void Init()
        {
        }
   
        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            return new CharacterAttribute(fore.Value & (back.Value >> 31), attrs);
        }
        
        


        private (TvColor fore, TvColor back) DecodeColorsFromAttribute(CharacterAttribute attr)
        {
            var fvalue = (int) (attr.ColorIdx & 0xffffffff);
            var bvalue = (int) ((attr.ColorIdx << 31) & 0xffffffff);
            return (TvColor.FromRaw(fvalue), TvColor.FromRaw(bvalue));
        }
        

        public CharacterAttribute DefaultAttribute { get; }


        public void SetAttributes(CharacterAttribute attributes)
        {
            var (fore, back) = DecodeColorsFromAttribute(attributes);
            var (fr, fg, fb) = fore.Rgb;
            var foreString = string.Format(setafFormat, fr.ToString(), fg.ToString(), fb.ToString());
            TerminfoBindings.putp(foreString);
            var (br, bg, bb) = back.Rgb;
            var backString = string.Format(setabFormat, br.ToString(), bg.ToString(), bb.ToString());
            TerminfoBindings.putp(backString);
        }
    }
}