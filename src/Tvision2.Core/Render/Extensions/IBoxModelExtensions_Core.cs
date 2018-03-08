using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render.Extensions
{
    public static class IBoxModelExtensions_Core
    {
        public static TvPoint ViewPointToConsolePoint(this IBoxModel boxModel, TvPoint viewpoint) => Viewport.ViewPointToConsolePoint(viewpoint, boxModel.Position);
        public static TvPoint ConsolePointToViewport(this IBoxModel boxModel, TvPoint consolepoint) => Viewport.ConsolePointToViewport(consolepoint, boxModel.Position);
        public static bool IsConsolePointInside(this IBoxModel boxModel, TvPoint consolepoint) => Viewport.IsConsolePointInside(consolepoint, boxModel.Position);
    }
}
