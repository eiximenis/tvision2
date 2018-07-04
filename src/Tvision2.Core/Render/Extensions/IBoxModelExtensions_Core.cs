using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render.Extensions
{
    public static class IBoxModelExtensions_Core
    {
        public static TvPoint ViewPointToConsolePoint(this IViewport boxModel, TvPoint viewpoint) => ViewportHelper.ViewPointToConsolePoint(viewpoint, boxModel.Position);
        public static TvPoint ConsolePointToViewport(this IViewport boxModel, TvPoint consolepoint) => ViewportHelper.ConsolePointToViewport(consolepoint, boxModel.Position);
        public static bool IsConsolePointInside(this IViewport boxModel, TvPoint consolepoint) => ViewportHelper.IsConsolePointInside(consolepoint, boxModel.Position);
    }
}
