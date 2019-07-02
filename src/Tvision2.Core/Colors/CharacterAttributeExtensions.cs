using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public static class CharacterAttributeExtensions
    {

        public static (TvColor fore, TvColor back) DecodeColorsFromAttribute(this CharacterAttribute attr)
        {
            return (attr.Fore, attr.Back);
        }

        public static CharacterAttribute EncodeColorsInAttribute(TvColor fore, TvColor back, CharacterAttributeModifiers attrs)
        {
            return new CharacterAttribute(new TvColorPair(fore, back), attrs);
        }

    }
}
