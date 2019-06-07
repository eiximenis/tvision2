using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public static class CharacterAttributeExtensions
    {
        private const ulong FORECOL_MASK = 0x00000000FFFFFFFF;

        public static (TvColor fore, TvColor back) DecodeColorsFromAttribute(this CharacterAttribute attr)
        {
            var fvalue = (int)(attr.ColorIdx & FORECOL_MASK);
            var bvalue = (int)(attr.ColorIdx >> 32);
            return (TvColor.FromRaw(fvalue), TvColor.FromRaw(bvalue));
        }

        public static CharacterAttribute EncodeColorsInAttribute(TvColor fore, TvColor back, CharacterAttributeModifiers attrs)
        {
            return new CharacterAttribute((ulong)fore.Value | ((ulong)back.Value << 32), attrs);
        }

    }
}
