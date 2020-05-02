using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Tvision2.ConsoleDriver.ColorDefinitions;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver.Common
{
    class NcursesPalette : BasePalette, IPalette
    {

        private readonly PaletteOptions _options;
        
        public bool IsFreezed { get; private set; }

        public ColorMode ColorMode { get; private set; }
        
        public NcursesPalette(PaletteOptions options) 
        {
            IsFreezed = true;
            MaxColors = 0;
            ColorMode =  options.ForceBasicPalette ? ColorMode.Basic : ColorMode.Palettized;
            _options = options;
        }

        public bool RedefineColor(int idx, TvColor newColor)
        {
            if (IsFreezed || !newColor.IsRgb)
            {
                return false;
            }

            var (red, green, blue) = newColor.Rgb;
            Curses.InitColor((short) idx, (short) red, (short) green, (short) blue);
            SetColorAt(idx, newColor);
            return true;
        }

        internal void Init()
        {
            if (Curses.HasColors)
            {
                Curses.StartColor();
                MaxColors = Curses.Colors;
                InitSize(MaxColors);
                if (!string.IsNullOrEmpty(_options.PaletteToLoad))
                {
                    LoadPalette(_options.PaletteToLoad, _options.PaletteParser, _options.UpdateTerminalEntries);
                }
                else
                {
                    LoadPalette("ansi", DefaultPaletteDefinitionParser.Instance, _options.UpdateTerminalEntries);
                }

                IsFreezed = ColorMode == ColorMode.Basic || !Curses.CanChangeColor();
            }
            else
            {
                ColorMode = ColorMode.NoColors;
                MaxColors = 2;
                InitSize(2);
                AddColor(0, TvColor.Black,TvColorNames.NameOf(TvColorNames.Black));
                AddColor(1, TvColor.White,TvColorNames.NameOf(TvColorNames.White));
                IsFreezed = true;
            }
        }

        protected override  void OnColorAdded(TvColor color, int idx)
        {
            if (!IsFreezed)
            {
                var (cr, cg, cb) = color.Rgb;
                var (r, g, b) = ((short) (cr / 256f * 1000), (short) (cg / 256f * 1000), (short) (cb / 256f * 1000));

                Curses.InitColor((short) idx, r, g, b);
            }
        }

        
    }
}