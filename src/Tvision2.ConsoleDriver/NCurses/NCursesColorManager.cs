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


        private readonly Dictionary<(int fore, int back), int> _pairs;
        private int _lastPairUsedIdx;

        public CharacterAttribute DefaultAttribute { get; }

        public IPalette Palette => _palette;

        private readonly IRgbColortranslator _colorTranslator;



        public NcursesColorManager(PaletteOptions options)
        {
            _useBgHilite = true;        // Todo: Need option to change this.
            _pairs = new Dictionary<(int fore, int back), int>();
            _lastPairUsedIdx = 0;
            DefaultAttribute = new CharacterAttribute(new TvColorPair(TvColor.White, TvColor.Black), CharacterAttributeModifiers.Normal);
            GetPairIndexFor(DefaultAttribute.Fore, DefaultAttribute.Back);
            _palette = new NcursesPalette(options);
            _colorTranslator = options.ColorTranslator;
        }

        public int GetPairIndexFor(TvColor fore, TvColor back)
        {
            var foreColorNumber = GetColorNumber(fore);
            var backColorNumber = GetColorNumber(back);
            
            if (_pairs.TryGetValue((foreColorNumber, backColorNumber), out int pairidx))
            {
                return pairidx;
            }

            _lastPairUsedIdx++;


            Curses.init_pair((short)_lastPairUsedIdx, (short)foreColorNumber, (short)backColorNumber);
            _pairs.Add((foreColorNumber, backColorNumber), _lastPairUsedIdx);
            var lastPair = _lastPairUsedIdx;
            if (_useBgHilite && back.IsBasic)
            {
                _lastPairUsedIdx++;
                Curses.init_pair((short)_lastPairUsedIdx, (short)foreColorNumber, (short)(backColorNumber  + TvColorNames.StandardColorsCount));
                _pairs.Add((foreColorNumber, backColorNumber + TvColorNames.StandardColorsCount), _lastPairUsedIdx);
            }

            return lastPair;
        }

        private int GetColorNumber(TvColor color)
        {
            if (!color.IsRgb) return color.PaletteIndex;

            if (_colorTranslator != null)
            {
                return _colorTranslator.GetColorIndexFromRgb(color, _palette);
            }

            throw new InvalidOperationException(
                $"RGB color passed {color.ToString()} and no color translator is set. Do not use RGB colors or use  TranslateRgbColorsWith in the WithPalette setting");
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
