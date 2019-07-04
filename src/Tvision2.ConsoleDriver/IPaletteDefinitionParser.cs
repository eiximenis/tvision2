using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver
{
    public interface IPaletteDefinitionParser
    {
        (string name, int idx, TvColor rgbColor) ParseLine(string line);
    }
}