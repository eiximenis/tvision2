using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.ColorTranslators
{
    public class InterpolationPaletteTranslator : IRgbColortranslator
    {
        private readonly Dictionary<TvColor, int> _translatedColors;
         

        public InterpolationPaletteTranslator()
        {
            _translatedColors = new Dictionary<TvColor, int>();             // Todo: Can grow ad-eternum... Need to limit max dictionary entries (using something like LRU) 
        }
        
        public int GetColorIndexFromRgb(TvColor rgbColor, IPalette palette)
        {
            var selectedIdx = 0;
            var mindiff = int.MaxValue;
            foreach (var color in palette.Entries)
            {
                var diff = color.rgbColor.Diff(rgbColor);
                var diffAbs = Math.Abs(diff.blue) + Math.Abs(diff.green) + Math.Abs(diff.red);
                if (diffAbs < mindiff)
                {
                    mindiff = diffAbs;
                    selectedIdx = color.idx;
                }
            }

            return selectedIdx;
        }
    }
}