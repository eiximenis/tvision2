using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Core.Render
{
    public static class ViewportHelper
    {
        public static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
        public static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;

        public static bool IsConsolePointInside(TvPoint consolePoint, TvPoint viewportPosition)
        {
            return consolePoint.Top >= viewportPosition.Top && consolePoint.Left >= viewportPosition.Left;
        }

        public static void Fill(CharacterAttribute attr, IViewport viewport, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(TvPoint.FromXY(0, 0), viewport.Position);
            var vpRows = viewport.Bounds.Rows;
            for (var rows = 0; rows < vpRows; rows++)
            {
                console.DrawAt(new string(' ', viewport.Bounds.Cols), location + TvPoint.FromXY(0, rows), (int)viewport.ZIndex, attr);
            }
        }

        public static void Clear(IViewport viewport, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(TvPoint.FromXY(0, 0), viewport.Position);
            console.DrawAt(new string(' ', viewport.Bounds.Cols), location, int.MinValue, new CharacterAttribute());
        }
    }
}
