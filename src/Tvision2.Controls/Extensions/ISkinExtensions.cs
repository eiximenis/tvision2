using System;
using Tvision2.Controls;

namespace Tvision2.Styles
{
    public static class ISkinExtensions
    {
        public static IStyle GetControlStyle(this ISkin skin, ITvControl control) => skin[control.ControlType];
    }
}
