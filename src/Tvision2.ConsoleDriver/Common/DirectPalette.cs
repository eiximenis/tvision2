using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.ColorDefinitions;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    class DirectPalette : BasePalette, IPalette
    {
        private readonly PaletteOptions _options;
        public bool IsFreezed => true;

        public ColorMode ColorMode => ColorMode.Direct;
        
        public bool RedefineColor(int idx, TvColor newColor) => false;

        public DirectPalette(int size, PaletteOptions options)
        {
            _options = options;
            InitSize(size);
            if (!string.IsNullOrEmpty(_options.PaletteToLoad))
            {
                LoadPalette(_options.PaletteToLoad, _options.PaletteParser, _options.UpdateTerminalEntries);
            }
            else
            {
                LoadPalette("ansi", DefaultPaletteDefinitionParser.Instance, _options.UpdateTerminalEntries);
            }

        }

    }
}
