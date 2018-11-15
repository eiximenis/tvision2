using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver.NCurses
{
    public class NcursesColorManager : IColorManager
    {

        private bool _supportsBgHilite;

        public int MaxColors { get; private set; }

        public int MaxPairs { get; private set; }

        private readonly Dictionary<(int fore, int back), int> _pairs;
        private int _lastPairUsedIdx;

        public CharacterAttribute DefaultAttribute  { get; }


        public NcursesColorManager()
        {
            _supportsBgHilite = false;
            _pairs = new Dictionary<(int fore, int back), int>();
            _lastPairUsedIdx = 0;
            DefaultAttribute = new CharacterAttribute()
            {
                ColorIdx = 0,
                Modifiers = CharacterAttributeModifiers.Normal
            };
        }

        public int GetPairIndexFor(DefaultColorName fore, DefaultColorName back)
        {
            if (_pairs.TryGetValue(((int)fore, (int)back), out int pairidx))
            {
                return pairidx;
            }

            _lastPairUsedIdx++;
            Curses.init_pair((short)_lastPairUsedIdx, (short) fore, (short) back);
            _pairs.Add(((int) fore, (int) back), _lastPairUsedIdx);
            var lastPair = _lastPairUsedIdx;
            if (_supportsBgHilite)
            {
                _lastPairUsedIdx++;
                // TODO: Instead fore+1 & back+1 use redefined color indexes (fore + 8, back + 8)
                Curses.init_pair((short)_lastPairUsedIdx, (short)(fore + 1), (short)(back + 1));
                _pairs.Add(((int)fore + 1, (int)back + 1), _lastPairUsedIdx);
            }


            return lastPair;
        }


        internal void Init()
        {
            if (Curses.HasColors)
            {
                Curses.StartColor();
                MaxColors = Curses.Colors;
                MaxPairs = Curses.ColorPairs;
                if (true)       // TODO: Ask ncurses if colors can be redefined
                {
                    CreateHiliteBgColors();
                    _supportsBgHilite = true;
                }
            }
        }

        private void CreateHiliteBgColors()
        {
            // TODO: Create hilitecolors & store in 9-15
 
        }

        public void SetColor(int pairIdx)
        {
            Curses.attrset(Curses.ColorPair(pairIdx));
        }


        
        public CharacterAttribute BuildAttributeFor(DefaultColorName fore, DefaultColorName back, 
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

            return new CharacterAttribute()
            {
                ColorIdx = coloridx,
                Modifiers = attrs
            };
        }

        public void SetAttributes(CharacterAttribute attributes)
        {
            Curses.attrset(Curses.ColorPair(attributes.ColorIdx) | ((int)attributes.Modifiers << 8));
        }
    }
}
