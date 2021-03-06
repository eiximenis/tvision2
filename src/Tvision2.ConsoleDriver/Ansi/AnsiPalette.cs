﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.ColorDefinitions;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Ansi
{
    class AnsiPalette : BasePalette, IPalette
    {
        private readonly PaletteOptions _options;
        public bool IsFreezed => true;


        public ColorMode ColorMode { get; }
        
        public bool RedefineColor(int idx, TvColor newColor) => false;

        public AnsiPalette(int size, PaletteOptions options)
        {
            _options = options;
            InitSize(size);
            if (_options.ForceBasicPalette)
            {
                ColorMode = ColorMode.Basic;
            }
            else
            {
                ColorMode = _options.TrueColorEnabled ? ColorMode.Direct : ColorMode.Palettized;
            }
        }

        public void Init()
        {
            if (!string.IsNullOrEmpty(_options.PaletteToLoad))
            {
                LoadPalette(_options.PaletteToLoad, _options.PaletteParser, _options.UpdateTerminalEntries);
            }
            else
            {
                LoadPalette("ansi", DefaultPaletteDefinitionParser.Instance, _options.UpdateTerminalEntries);
            }            
        }

        protected override void OnColorAdded(TvColor color, int idx)
        {
            
            var (r, g, b) = color.Rgb;
            Console.Out.Write(string.Format(AnsiEscapeSequences.INITC, idx, r, g, b));
            Console.Out.Flush();
        }
    }
}
