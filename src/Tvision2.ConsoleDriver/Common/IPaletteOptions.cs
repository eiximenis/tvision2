using System;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    public interface IPaletteOptions
    {
        IPaletteOptionsWithPaletteInitialized InitFromTerminalName(string name = null);
        IPaletteOptionsWithPaletteInitialized InitFromDefinition();
    }

    public interface IPaletteOptionsWithPaletteInitialized
    {
        IPaletteOptionsWithPaletteInitialized UpdateTerminal(bool update=true);
        IPaletteOptionsWithPaletteInitialized ParseWith(IPaletteDefinitionParser parser);
        IPaletteOptionsWithPaletteInitialized ParseWith(Func<string, (string name, int idx, TvColor rgbColor)> parserFunc);

        IPaletteOptionsWithPaletteInitialized TranslateRgbColorsWith(IRgbColortranslator translator);
        IPaletteOptionsWithPaletteInitialized TranslateRgbColorsWith(Func<TvColor, IPalette, int> translatorFunc);
    }
}