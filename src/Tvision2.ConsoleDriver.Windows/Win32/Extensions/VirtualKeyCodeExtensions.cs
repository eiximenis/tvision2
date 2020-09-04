using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.ConsoleDriver.Win32
{
    public static class VirtualKeyCodeExtensions
    {
        public static bool IsModifierKey(this VirtualKeys vkey)
        {
            return  !(vkey == VirtualKeys.Shift || vkey == VirtualKeys.LeftShift || vkey == VirtualKeys.RightShift) &&
                    !(vkey == VirtualKeys.Control || vkey == VirtualKeys.LeftControl || vkey == VirtualKeys.RightControl) &&
                    !(vkey == VirtualKeys.Menu || vkey == VirtualKeys.LeftMenu || vkey == VirtualKeys.RightMenu);
        }
    }
}
