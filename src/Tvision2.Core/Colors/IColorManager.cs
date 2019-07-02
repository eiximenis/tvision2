using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public interface IColorManager
    {

        CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back, CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal);
        CharacterAttribute DefaultAttribute { get; }

        IPalette Palette { get; }

    }   
}
