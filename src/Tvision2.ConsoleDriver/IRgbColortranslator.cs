using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver
{
    public interface IRgbColortranslator
    {
        int GetColorIndexFromRgb(TvColor rgbColor, IPalette palette);
    }
}