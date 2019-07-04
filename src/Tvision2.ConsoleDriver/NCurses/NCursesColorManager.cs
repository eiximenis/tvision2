using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tvision2.Core.Colors;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver.Common
{
    public class NcursesColorManager : IColorManager
    {

        private bool _useBgHilite;
        private readonly NcursesPalette _palette;
        public int MaxPairs { get; private set; }


        private readonly Dictionary<(TvColor fore, TvColor back), int> _pairs;
        private int _lastPairUsedIdx;

        public CharacterAttribute DefaultAttribute { get; }

        public IPalette Palette => _palette;



        public NcursesColorManager(PaletteOptions options)
        {
            _useBgHilite = true;        // Todo: Need option to change this.
            _pairs = new Dictionary<(TvColor fore, TvColor back), int>();
            _lastPairUsedIdx = 0;
            DefaultAttribute = new CharacterAttribute(new TvColorPair(TvColor.White, TvColor.Black), CharacterAttributeModifiers.Normal);
            GetPairIndexFor(DefaultAttribute.Fore, DefaultAttribute.Back);
            _palette = new NcursesPalette(options);
        }

        public int GetPairIndexFor(TvColor fore, TvColor back)
        {
            
            
            
            if (_pairs.TryGetValue((fore, back), out int pairidx))
            {
                return pairidx;
            }

            _lastPairUsedIdx++;

            var foreColorNumber = GetColorNumber(fore);
            var backColorNumber = GetColorNumber(back);

            Curses.init_pair((short)_lastPairUsedIdx, (short)foreColorNumber, (short)backColorNumber);
            _pairs.Add((fore, back), _lastPairUsedIdx);
            var lastPair = _lastPairUsedIdx;
            if (_useBgHilite)
            {
                _lastPairUsedIdx++;
                Curses.init_pair((short)_lastPairUsedIdx, (short)(fore), (short)(back.Plus(TvColorNames.StandardColorsCount)));
                _pairs.Add((fore, back.Plus(TvColorNames.StandardColorsCount)), _lastPairUsedIdx);
            }

            return lastPair;
        }

        private int GetColorNumber(TvColor color)
        {
            if (!color.IsRgb) return color.PaletteIndex;
            
            return 2;
        }

        internal void Init()
        {
            MaxPairs = Curses.ColorPairs;
            _palette.Init();
        }



        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            var coloridx = GetPairIndexFor(fore, back);

            if (_useBgHilite)
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
            var pairIdx = GetPairIndexFor(attributes.Fore, attributes.Back);
            var attr = (int)(attributes.Modifiers) & ~(int)CharacterAttributeModifiers.BackgroundBold;
            Curses.attrset(Curses.ColorPair(pairIdx | (attr << 8)));
        }
    }
}
