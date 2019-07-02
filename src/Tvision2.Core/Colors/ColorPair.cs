using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public struct TvColorPair
    {
        public TvColor ForeGround { get;  }
        public TvColor Background { get; }
        public TvColorPair(TvColor fore, TvColor back)
        {
            ForeGround = fore;
            Background = back;
        }


    }
}
