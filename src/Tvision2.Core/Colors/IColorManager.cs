using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public interface IColorManager
    {
        int MaxColors { get; }

        int GetPairIndexFor(DefaultColorName fore, DefaultColorName back);
    }
}
