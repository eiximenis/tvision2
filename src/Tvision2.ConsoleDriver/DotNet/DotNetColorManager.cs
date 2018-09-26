using System;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Colors
{
    class DotNetColorManager : IColorManager
    {
        private const int DOTNET_MAX_COLORS = 8;        // On DotNet only support 8 basic colors
        private const int DOTNET_MAX_PAIRS = 64;        // In .net all basic combinations are allowed, thus 8 fg * 8 bg

        public int MaxColors => DOTNET_MAX_COLORS;

        public int MaxPairs => 64;

        private readonly ColorPair[] _pairs;
        private readonly ConsoleColor[] _dotnetMap;

        public DotNetColorManager()
        {
            _dotnetMap = new ConsoleColor[DOTNET_MAX_COLORS];

            foreach (DefaultColorName stdcolor in Enum.GetValues(typeof(DefaultColorName)))
            {
                switch (stdcolor)
                {
                    case DefaultColorName.Black: _dotnetMap[(int)stdcolor] = ConsoleColor.Black; break;
                    case DefaultColorName.Blue: _dotnetMap[(int)stdcolor] = ConsoleColor.Blue; break;
                    case DefaultColorName.Cyan: _dotnetMap[(int)stdcolor] = ConsoleColor.Cyan; break;
                    case DefaultColorName.Green: _dotnetMap[(int)stdcolor] = ConsoleColor.Green; break;
                    case DefaultColorName.Magenta: _dotnetMap[(int)stdcolor] = ConsoleColor.Magenta; break;
                    case DefaultColorName.Red: _dotnetMap[(int)stdcolor] = ConsoleColor.Red; break;
                    case DefaultColorName.White: _dotnetMap[(int)stdcolor] = ConsoleColor.White; break;
                    case DefaultColorName.Yellow: _dotnetMap[(int)stdcolor] = ConsoleColor.Yellow; break;
                }
            }

            _pairs = new ColorPair[DOTNET_MAX_PAIRS];
            for (var fore = DOTNET_MAX_COLORS - 1; fore >= 0 ; fore--)
            {
                for (var back = 0; back < DOTNET_MAX_COLORS; back++)
                {
                    _pairs[fore + (back << 3)] = new ColorPair(fore, back);
                }
            }
        }

        public ColorPair this[int idx] => _pairs[idx];

        public (ConsoleColor fore, ConsoleColor back) ColorPairToDotNetColors(int pairIdx)
        {
            var pair = _pairs[pairIdx];
            return (_dotnetMap[pair.ForeGround], _dotnetMap[pair.Background]);
        }

        public int GetPairIndexFor(DefaultColorName fore, DefaultColorName back) => (int)fore + ((int)back << 3);
    }
}
