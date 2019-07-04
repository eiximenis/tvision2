using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    class DirectPalette : BasePalette, IPalette
    {
        public bool IsFreezed => true;

        public ColorMode ColorMode => ColorMode.Direct;
        
        public bool RedefineColor(int idx, TvColor newColor) => false;

        public DirectPalette(int size)
        {
            InitSize(size);
        }
        
        
        
        
    }
}
