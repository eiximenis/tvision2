using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public interface IColorManager
    {
        int MaxColors { get; }
        int MaxPairs { get; }

        int GetPairIndexFor(TvisionColor fore, TvisionColor back);
        CharacterAttribute BuildAttributeFor(TvisionColor fore, TvisionColor back, CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal);
        CharacterAttribute DefaultAttribute { get; }
    }   
}
