using System;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    public interface IPaletteOptions
    {
        IPaletteOptionsWithPaletteInitialized LoadFromTerminalName(string name = null);
        IPaletteOptionsWithPaletteInitialized LoadFromDefinition();

        void UseBasicColorMode();
        
        void TranslateRgbColorsWith(IRgbColortranslator translator);
        void TranslateRgbColorsWith(Func<TvColor, IPalette, int> translatorFunc);
    }

    public interface IPaletteOptionsWithPaletteInitialized
    {
        IPaletteOptionsWithPaletteInitialized UpdateTerminal(UpdateTerminalEntries entriesToUpdate = UpdateTerminalEntries.AllButAnsi4bit);
        IPaletteOptionsWithPaletteInitialized ParseWith(IPaletteDefinitionParser parser);
        IPaletteOptionsWithPaletteInitialized ParseWith(Func<string, (string name, int idx, TvColor rgbColor)> parserFunc);


    }
}