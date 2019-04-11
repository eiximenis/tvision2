﻿using System;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Colors
{
    class DotNetColorManager : IColorManager
    {
        private const int DOTNET_MAX_COLORS = 8; // On DotNet only support 8 basic colors (+ 8 with "Bold Attribute")
        private const int DOTNET_MAX_PAIRS = 256; // In .net all basic combinations are allowed, thus 8 fg * 8 bg

        public int MaxColors => DOTNET_MAX_COLORS;

        public int MaxPairs => 64;
        public CharacterAttribute DefaultAttribute { get; }

        private readonly ColorPair[] _pairs;
        private readonly ConsoleColor[,] _dotnetMap;

        public DotNetColorManager()
        {
            _dotnetMap = new ConsoleColor[DOTNET_MAX_COLORS, 2];

            foreach (var stdcolor in TvColorNames.AllStandardColors())
            {
                switch (stdcolor)
                {
                    case TvColorNames.Black:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.Black;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.DarkGray;
                        break;
                    case TvColorNames.Blue:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.DarkBlue;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.Blue;
                        break;
                    case TvColorNames.Cyan:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.DarkCyan;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.Cyan;
                        break;
                    case TvColorNames.Green:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.DarkGreen;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.Green;
                        break;
                    case TvColorNames.Magenta:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.DarkMagenta;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.Magenta;
                        break;
                    case TvColorNames.Red:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.DarkRed;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.Red;
                        break;
                    case TvColorNames.White:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.Gray;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.White;
                        break;
                    case TvColorNames.Yellow:
                        _dotnetMap[(int)stdcolor, 0] = ConsoleColor.DarkYellow;
                        _dotnetMap[(int)stdcolor, 1] = ConsoleColor.Yellow;
                        break;
                }
            }

            _pairs = new ColorPair[TvColorNames.StandardColorsCount];
            for (var fore = TvColorNames.StandardColorsCount - 1; fore >= 0; fore--)
            {
                for (var back = 0; back < TvColorNames.StandardColorsCount; back++)
                {
                    _pairs[fore + (back << 3)] = new ColorPair(new TvColor(fore), new TvColor(back));
                }
            }

            DefaultAttribute = BuildAttributeFor(TvColor.White, TvColor.Black, CharacterAttributeModifiers.Normal);
        }

        public ColorPair this[int idx] => _pairs[idx];

        public (ConsoleColor fore, ConsoleColor back) AttributeToDotNetColors(CharacterAttribute attribute)
        {
            var pair = _pairs[attribute.ColorIdx];
            var bright = (attribute.Modifiers | CharacterAttributeModifiers.Bold) != 0;
            return (_dotnetMap[pair.ForeGround.Value, bright ? 1 : 0], _dotnetMap[pair.Background.Value, bright ? 1 : 0]);
        }

        public int GetPairIndexFor(TvColor fore, TvColor back) => (int)fore + ((int)back << 3);

        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal) =>
             new CharacterAttribute((int) fore + ((int) back << 3), attrs);
    }
}