using Microsoft.Extensions.Logging.Internal;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public class TerminfoTrueColorManager : ITerminfoColorManager
    {

        private string _setaf;
        private string _setab;
        private const int MAXPALETTESIZE = 256; 


        public IPalette Palette { get; }

        public TerminfoTrueColorManager(PaletteOptions options)
        {
            Palette = new DirectPalette(MAXPALETTESIZE, options);
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
            return CharacterAttributeExtensions.EncodeColorsInAttribute(fore, back, attrs);
        }


        public CharacterAttribute DefaultAttribute { get; }


        public void SetAttributes(CharacterAttribute attributes)
        {
            var (fore, back) = attributes.DecodeColorsFromAttribute();
            var (fr, fg, fb) = fore.Rgb;
            var foreString = TerminfoBindings.tparm(_setaf, fr, fg, fb);
            TerminfoBindings.putp(foreString);
            var (br, bg, bb) = back.Rgb;
            var backString = TerminfoBindings.tparm(_setab, br, bg, bb);
            TerminfoBindings.putp(backString);

        }
    }
}