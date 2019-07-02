using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tvision2.Core.Colors;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver.NCurses
{
    public class NcursesColorManager : IColorManager
    {

        private bool _supportsBgHilite;
        private readonly NcursesPalette _palette;

        public int MaxPairs { get; private set; }


        private readonly Dictionary<(TvColor fore, TvColor back), int> _pairs;
        private int _lastPairUsedIdx;

        public CharacterAttribute DefaultAttribute { get; }

        public IPalette Palette => _palette;


        public NcursesColorManager()
        {
            _supportsBgHilite = false;
            _pairs = new Dictionary<(TvColor fore, TvColor back), int>();
            _lastPairUsedIdx = 0;
            DefaultAttribute = new CharacterAttribute(new TvColorPair(TvColor.White, TvColor.Black), CharacterAttributeModifiers.Normal);
            GetPairIndexFor(DefaultAttribute.Fore, DefaultAttribute.Back);
            _palette = new NcursesPalette();
        }

        public int GetPairIndexFor(TvColor fore, TvColor back)
        {
            if (_pairs.TryGetValue((fore, back), out int pairidx))
            {
                return pairidx;
            }

            _lastPairUsedIdx++;
            Curses.init_pair((short)_lastPairUsedIdx, (short)fore, (short)back);
            _pairs.Add((fore, back), _lastPairUsedIdx);
            var lastPair = _lastPairUsedIdx;
            if (_supportsBgHilite)
            {
                _lastPairUsedIdx++;
                Curses.init_pair((short)_lastPairUsedIdx, (short)(fore), (short)(back.Plus(TvColorNames.StandardColorsCount)));
                _pairs.Add((fore, back.Plus(TvColorNames.StandardColorsCount)), _lastPairUsedIdx);
            }

            return lastPair;
        }


        internal void Init()
        {
            _palette.Init();
            MaxPairs = Curses.ColorPairs;
            if (!_palette.IsFreezed)
            {
                CreateHiliteBgColors();
                _supportsBgHilite = true;
            }
        }

        private void CreateHiliteBgColors()
        {
            Curses.InitColor((short)(TvColorNames.Black + 8), 0, 0, 0);
            Curses.InitColor((short)(TvColorNames.Red + 8), 800, 0, 0);
            Curses.InitColor((short)(TvColorNames.Green + 8), 0, 800, 0);
            Curses.InitColor((short)(TvColorNames.Yellow + 8), 800, 800, 0);
            Curses.InitColor((short)(TvColorNames.Blue + 8), 0, 0, 800);
            Curses.InitColor((short)(TvColorNames.Magenta + 8), 800, 0, 800);
            Curses.InitColor((short)(TvColorNames.Cyan + 8), 0, 800, 800);
            Curses.InitColor((short)(TvColorNames.White + 8), 800, 800, 800);

        }


        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            var coloridx = GetPairIndexFor(fore, back);

            if (_supportsBgHilite)
            {

                if ((attrs & CharacterAttributeModifiers.BackgroundBold) == CharacterAttributeModifiers.BackgroundBold)
                {
                    coloridx++;
                }
            }

            return new CharacterAttribute(new TvColorPair(fore, back), attrs);
        }

        public void SetAttributes(CharacterAttribute attributes)
        {
            var pairIdx = _pairs[(attributes.Fore, attributes.Back)];
            var attr = (int)(attributes.Modifiers) & ~(int)CharacterAttributeModifiers.BackgroundBold;
            Curses.attrset(Curses.ColorPair(pairIdx | (attr << 8)));
        }
    }
}
