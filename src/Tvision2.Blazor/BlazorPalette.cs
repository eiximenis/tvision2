using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.Blazor
{
    public class BlazorPalette :  IPalette
    {
        public bool IsFreezed => true;

        public ColorMode ColorMode { get; } = ColorMode.Direct;

        public int MaxColors { get; } = 0;

        public bool IsFull { get; } = true;

        public IEnumerable<(int idx, TvColor rgbColor)> Entries => Enumerable.Empty<(int idx, TvColor rgbColor)>();

        public TvColor this[int idx] => TvColor.Black;

        public TvColor this[string name] => TvColor.Black;

        public bool RedefineColor(int idx, TvColor newColor) => false;

        public int AddColor(TvColor color, string name = null)
        {
            return 0;
        }

        public BlazorPalette()
        {

        }


    }
}
