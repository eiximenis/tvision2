using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    class DefaultStyle : IStyle
    {
        static DefaultStyle() => Instance = new DefaultStyle();

        public static IStyle Instance { get; private set; }

        public ConsoleColor ForeColor => ConsoleColor.White;

        public ConsoleColor BackColor => ConsoleColor.Black;

        public ConsoleColor HiliteForeColor => ConsoleColor.Black;
        public ConsoleColor HiliteBackColor => ConsoleColor.White;
    }
}
