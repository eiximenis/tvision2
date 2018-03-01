using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    class DefaultStyle : IStyle
    {
        private static IStyle _instance;

        static DefaultStyle() => _instance = new DefaultStyle();

        public static IStyle Instance => _instance;

        public ConsoleColor ForeColor => ConsoleColor.White;

        public ConsoleColor BackColor => ConsoleColor.Black;
    }
}
