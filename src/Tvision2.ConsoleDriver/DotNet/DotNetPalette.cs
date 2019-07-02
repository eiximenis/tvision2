using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Colors
{
    class DotNetPalette : IPalette
    {
        public const int DOTNET_MAX_COLORS = 8; // On DotNet only support 8 basic colors (+ 8 with "Bold Attribute")
        public bool IsFreezed { get; }

        public int MaxColors { get; }

        public ColorMode ColorMode => ColorMode.Basic;

        public DotNetPalette()
        {
            IsFreezed = true;
            MaxColors = DOTNET_MAX_COLORS;
        }

        public bool RedefineColor(int idx, TvColor newColor) => false;
    }
}