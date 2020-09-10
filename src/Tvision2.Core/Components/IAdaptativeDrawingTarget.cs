using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;

namespace Tvision2.Core.Components
{
    public interface IAdaptativeDrawingTarget
    {
        IAdaptativeDrawingTarget AddDrawer(ITvDrawer drawer);
    }

    public interface IAdaptativeDrawingTarget<T>
    {
        IAdaptativeDrawingTarget<T> AddDrawer(ITvDrawer<T> drawer);
        IAdaptativeDrawingTarget<T> AddDrawer(Action<RenderContext<T>> drawer);
        IAdaptativeDrawingTarget<T> AddDrawer<U>(ITvDrawer<U> drawer, Func<T, U> stateConverter);
    }
}
