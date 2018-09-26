using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    class Style : IStyle
    {
        private int _standard;
        private int _focused;

        public int Standard { get; internal set; }

        public int Focused { get; internal set; }


    }
}
