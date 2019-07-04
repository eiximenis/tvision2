using System;
using System.Data;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver
{
    public class DelegateRgbColorTranslator : IRgbColortranslator
    {

        private readonly Func<TvColor, IPalette, int> _translatorFunc;

        public DelegateRgbColorTranslator(Func<TvColor, IPalette, int> translatorFunc)
        {
            _translatorFunc = translatorFunc;
        }

        public int GetColorIndexFromRgb(TvColor rgbColor, IPalette palette) => _translatorFunc(rgbColor, palette);

    }
}