using System;
using System.Collections.Generic;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.ColorTranslators
{
    public class AddToPaletteTranslator : IRgbColortranslator
    {
        private readonly Dictionary<TvColor, int> _translatedColors;

        public AddToPaletteTranslator()
        {
            _translatedColors = new Dictionary<TvColor, int>();             
        }
        public int GetColorIndexFromRgb(TvColor rgbColor, IPalette palette)
        {
            if (palette.IsFull || palette.IsFreezed)
            {
                throw new InvalidOperationException(
                    $"Can't add color to palette. Is either full ({palette.IsFull}) or freezed ({palette.IsFreezed})");
            }
            
            if (_translatedColors.TryGetValue(rgbColor, out var existingIndex))
            {
                return existingIndex;
            }
            
            var index = palette.AddColor(rgbColor, rgbColor.ToString());
            _translatedColors.Add(rgbColor, index);
            return index;
        }
    }
}