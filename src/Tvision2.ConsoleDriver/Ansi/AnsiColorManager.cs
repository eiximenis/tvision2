using System;
using System.Dynamic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Tvision2.ConsoleDriver.Colors;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Ansi
{
    public class AnsiColorManager : IColorManager
    {

        private const string SGR_RGB_FORE = "\x1b[38;2;{0};{1};{2}m";                // Truecolor SETAF
        private const string SGR_RGB_BACK = "\x1b[48;2;{0};{1};{2}m";                // Truecolor SETAB

        private const string SGR_88_FORE = "\x1b[38;5;{0}m";                         // 256 colors SETAB
        private const string SGR_88_BACK = "\x1b[48;5;{0}m";                         // 256 colors SETAF
        private const string SGR_ANSI = "\x1b[{0}m";                                 // ANSI SETAF / SETAB
        
        private const string CUP = "\x1b[{0};{1}H";

        private const int MAXPALETTESIZE = 256;

        private readonly AnsiPalette _palette;
        private readonly IRgbColortranslator _colorTranslator;

        public IPalette Palette
        {
            get => _palette;
        }

        public CharacterAttribute DefaultAttribute { get; }
        
        public AnsiColorManager(PaletteOptions options)
        {
            DefaultAttribute = BuildAttributeFor(TvColor.White, TvColor.Black, CharacterAttributeModifiers.Normal);
            _colorTranslator = options.ColorTranslator;
            _palette = new AnsiPalette(MAXPALETTESIZE, options);
        }

        public void Init()
        {
            _palette.Init();
        }
   
        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            return CharacterAttributeExtensions.EncodeColorsInAttribute(fore, back, attrs);
        }

        private (TvColor fore, TvColor back) DecodeColorsFromAttribute(CharacterAttribute attr)
        {
            return (attr.Fore, attr.Back);
        }


        public string GetAttributeSequence(CharacterAttribute attributes)
        {
            var (fore, back) = DecodeColorsFromAttribute(attributes);

            var forestring = (string) null;
            var backstring = (string)null;

            if (fore.IsRgb)
            {
                TvColor finalFore = fore;
                if (_palette.ColorMode != ColorMode.Direct)
                {
                    if (_colorTranslator != null) {

                        finalFore = _palette[_colorTranslator.GetColorIndexFromRgb(fore, _palette)];
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"RGB Color received: {fore.ToString()} but ColorMode is {_palette.ColorMode} and no ColorTranslator has been provided. Use TranslateRgbColorsWith in the WithPalette setting");
                    }                    
                }

                var (fr, fg, fb) = finalFore.Rgb;
                forestring = string.Format(SGR_RGB_FORE, fr.ToString(), fg.ToString(), fb.ToString());
            }

            else if (fore.IsPalettized)
            {
                forestring = string.Format(SGR_88_FORE, fore.PaletteIndex);
            }
            else
            {
                forestring = string.Format(SGR_ANSI, fore.Value + 30);
            }
            
            if (back.IsRgb)
            {
                TvColor finalBack = back;
                if (_palette.ColorMode != ColorMode.Direct )
                {
                    if (_colorTranslator != null)
                    {
                        finalBack = _palette[_colorTranslator.GetColorIndexFromRgb(back, _palette)];
                    }                
                    else
                    {
                        throw new InvalidOperationException(
                            $"RGB Color received: {back.ToString()} but ColorMode is {_palette.ColorMode} and no ColorTranslator has been provided. Use TranslateRgbColorsWith in the WithPalette setting");
                    }
                }


                var (br, bg, bb) = finalBack.Rgb;
                backstring = string.Format(SGR_RGB_BACK, br.ToString(), bg.ToString(), bb.ToString());
            }

            else if (back.IsPalettized)
            {
                backstring = string.Format(SGR_88_BACK, back.PaletteIndex);
            }
            else
            {
                backstring = string.Format(SGR_ANSI, back.Value + 40);
            }

            return forestring + backstring;
        }
        
        public string GetCursorSequence(int x, int y)
        {
            return string.Format(CUP, y + 1, x + 1);
        }
    }
}