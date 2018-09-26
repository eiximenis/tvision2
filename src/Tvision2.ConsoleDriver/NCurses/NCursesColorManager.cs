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

        public int GetPairIndexFor(DefaultColorName fore, DefaultColorName back)
        {
            throw new NotImplementedException();
        }

        internal void Init()
        {
            if (Curses.HasColors)
            {
                Curses.StartColor();
            }
        }
    }
}
