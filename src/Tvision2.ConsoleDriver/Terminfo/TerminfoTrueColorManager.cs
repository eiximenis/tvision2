using Microsoft.Extensions.Logging.Internal;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public class TerminfoTrueColorManager : ITerminfoColorManager
    {

        private  string _setaf;
        private  string _setab;
        
        
        public int MaxColors { get; }
        
        public TerminfoTrueColorManager()
        {
            MaxColors = 16 * 1024 * 1024;
        }

        public void Init()
        {
            _setaf = TerminfoBindings.tigetstr("setaf");
            _setab = TerminfoBindings.tigetstr("setab");


            var x = TerminfoBindings.tigetstr("RGB");
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
            var foreString = TerminfoBindings.tparm(_setaf, fr, fg, fb);
            TerminfoBindings.putp(foreString);
            var (br, bg, bb) = back.Rgb;
            var backString = TerminfoBindings.tparm(_setab, br, bg, bb);
            TerminfoBindings.putp(backString);
            
        }
    }
}