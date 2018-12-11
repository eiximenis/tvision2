using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    public interface IViewportFactory
    {
        IViewport FullViewport();
        IViewport BottomViewport(int vprows = 1);
    }
}
