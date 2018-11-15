using System.Collections.Generic;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Win32
{
    class Win32StdColorManager : IWindowsColorManager
    {

        class Win32StdColorPair
        {
            public int Idx { get; }
            public int WinColor { get; }

            public Win32StdColorPair(int idx, int color)
            {
                Idx = idx;
                WinColor = color;
            }
        }

        public int MaxColors => 8;
        public int MaxPairs => 64;

        private int _currentLastPair = -1;

        public CharacterAttribute DefaultAttribute { get; }

        private readonly Dictionary<(TvisionColor fore, TvisionColor back), Win32StdColorPair> _pairs;
        private List<Win32StdColorPair> _indexedPairs;

        public Win32StdColorManager()
        {
            _pairs = new Dictionary<(TvisionColor fore, TvisionColor back), Win32StdColorPair>();
            _indexedPairs = new List<Win32StdColorPair>();
            AddColorPair(TvisionColor.White, TvisionColor.Black);
            DefaultAttribute = BuildAttributeFor(TvisionColor.White, TvisionColor.Black, CharacterAttributeModifiers.Normal);
        }


        public CharacterAttribute BuildAttributeFor(TvisionColor fore, TvisionColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            return new CharacterAttribute()
            {
                ColorIdx = GetPairIndexFor(fore, back),
                Modifiers = attrs
            };
        }

        private Win32StdColorPair AddColorPair(TvisionColor fore, TvisionColor back)
        {
            _currentLastPair++;
            var wincolor = Win32ConsoleColor.ForeConsoleColorToAttribute(fore) | Win32ConsoleColor.BackConsoleColorToAttribute(back);
            var newPair = new Win32StdColorPair(_currentLastPair, wincolor);
            _pairs.Add((fore, back), newPair);
            _indexedPairs.Add(newPair);
            return newPair;
        }

        public int GetPairIndexFor(TvisionColor fore, TvisionColor back)
        {
            if (_pairs.TryGetValue((fore, back), out Win32StdColorPair pair))
            {
                return pair.Idx;
            }

            var newpair = AddColorPair(fore, back);
            return newpair.Idx;
        }

        public int AttributeToWin32Colors(CharacterAttribute attribute)
        {
            var winAttr = _indexedPairs[attribute.ColorIdx].WinColor;
            if ((attribute.Modifiers & CharacterAttributeModifiers.Bold) != 0)
            {
                winAttr |= (Win32ConsoleColor.FOREGROUND_INTENSITY);
            }

            if ((attribute.Modifiers & CharacterAttributeModifiers.BackgroundBold) != 0)
            {
                winAttr |= (Win32ConsoleColor.BACKGROUND_INTENSITY);
            }
            return winAttr;
        }
    }
}
