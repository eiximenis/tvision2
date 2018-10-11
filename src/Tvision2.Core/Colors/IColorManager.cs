using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public interface IColorManager
    {
        int MaxColors { get; }
        int MaxPairs { get; }

        int GetPairIndexFor(DefaultColorName fore, DefaultColorName back);
        CharacterAttribute BuildAttributeFor(DefaultColorName fore, DefaultColorName back, CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal);
    }   
}
