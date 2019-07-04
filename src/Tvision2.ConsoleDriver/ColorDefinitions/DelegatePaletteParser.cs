using System;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.ColorDefinitions
{
    public class DelegatePaletteParser : IPaletteDefinitionParser
    {
        private readonly Func<string, (string name, int idx, TvColor rgbColor)> _parserFunc;

        public DelegatePaletteParser(Func<string, (string name, int idx, TvColor rgbColor)> parserFunc)
        {
            _parserFunc = parserFunc;
        }

        public (string name, int idx, TvColor rgbColor) ParseLine(string line) => _parserFunc(line);

    }
}