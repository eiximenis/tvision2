using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public struct ColorPair
    {
        public TvColor ForeGround { get; set; }
        public TvColor Background { get; set; }

        public ColorPair(TvColor fore, TvColor back)
        {
            ForeGround = fore;
            Background = back;
        }
    }
}
