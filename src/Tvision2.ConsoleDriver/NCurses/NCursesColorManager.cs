using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver.NCurses
{
    public class NcursesColorManager : IColorManager
    {



        public int MaxColors { get; private set; }

        public int MaxPairs { get; private set; }

        private readonly Dictionary<(int fore, int back), int> _pairs;
        private int _lastPairUsedIdx;

        public CharacterAttribute DefaultAttribute  { get; }


        public NcursesColorManager()
        {
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
            return _lastPairUsedIdx;
        }


        internal void Init()
        {
            if (Curses.HasColors)
            {
                Curses.StartColor();
                MaxColors = Curses.Colors;
                MaxPairs = Curses.ColorPairs;
            }
        }

        public void SetColor(int pairIdx)
        {
            Curses.attrset(Curses.ColorPair(pairIdx));
        }


        
        public CharacterAttribute BuildAttributeFor(DefaultColorName fore, DefaultColorName back, 
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            return new CharacterAttribute()
            {
                ColorIdx = GetPairIndexFor(fore, back),
                Modifiers = attrs
            };
        }

        public void SetAttributes(CharacterAttribute attributes)
        {
            Curses.attrset(Curses.ColorPair(attributes.ColorIdx) | ((int)attributes.Modifiers << 8));
        }
    }
}
