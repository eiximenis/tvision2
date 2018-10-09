using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Win32
{
    public interface IWindowsColorManager : IColorManager
    {
        int AttributeToWin32Colors(CharacterAttribute attribute);
    }
}
