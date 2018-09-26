using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{
    public struct ColorPair
    {
        public int ForeGround { get; set; }
        public int Background { get; set; }

        public ColorPair(int fore, int back)
        {
            ForeGround = fore;
            Background = back;
        }
    }
}
