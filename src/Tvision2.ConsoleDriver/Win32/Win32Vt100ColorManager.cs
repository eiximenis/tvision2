using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Win32
{
    class Win32Vt100ColorManager : IWindowsColorManager
    {
        public int MaxColors => throw new NotImplementedException();

        public int MaxPairs => throw new NotImplementedException();

        public int AttributeToWin32Colors(CharacterAttribute attribute)
        {
            throw new NotImplementedException();
        }

        public CharacterAttribute BuildAttributeFor(DefaultColorName fore, DefaultColorName back, 
            CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            throw new NotImplementedException();
        }

        public int GetPairIndexFor(DefaultColorName fore, DefaultColorName back)
        {
            throw new NotImplementedException();
        }
    }
}
