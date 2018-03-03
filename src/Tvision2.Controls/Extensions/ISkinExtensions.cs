using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    public static class ISkinExtensions
    {
        public static IStyleSheet GetControlStyle(this ISkin skin, ITvControl control) => skin[control.ControlType];
    }
}
