using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    class DefaultBaseStyles : IBaseStyles
    {
        private static IBaseStyles _instance;

        static DefaultBaseStyles() => _instance = new DefaultBaseStyles();

        public static IBaseStyles Instance => _instance;

        public ConsoleColor ForeColor => ConsoleColor.White;

        public ConsoleColor BackColor => ConsoleColor.Black;
    }
}
