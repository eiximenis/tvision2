using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public static class Viewport
    {

        public static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition)
        {
            var top = viewPoint.Top + viewportPosition.Top;
            var left = viewPoint.Left + viewportPosition.Left;
            return new TvPoint(top, left);
        }

        public static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition)
        {
            var top = consolePoint.Top - viewportPosition.Top;
            var left = consolePoint.Left - viewportPosition.Left;
            return new TvPoint(top, left);
        }

        public static void DrawStringAt(string text, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor, IBoxModel boxModel, VirtualConsole console)
        {
            var clippingMode = boxModel.Clipping;
            var consoleLocation = ViewPointToConsolePoint(location, boxModel.Position);
            var zindex = boxModel.ZIndex;

            if ((clippingMode == ClippingMode.Clip || clippingMode == ClippingMode.ExpandVertical) && boxModel.Columns < text.Length)
            {
                text = text.Substring(0, boxModel.Columns);
            }

            console.DrawAt(text, consoleLocation, zindex, foreColor, backColor);
        }

        public static bool IsConsolePointInside(TvPoint consolePoint, TvPoint viewportPosition)
        {
            var viewPoint = ConsolePointToViewport(consolePoint, viewportPosition);
            return consolePoint.Top >= viewportPosition.Top && consolePoint.Left >= viewportPosition.Left;
        }

        public static void Fill(ConsoleColor color, IBoxModel boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            console.DrawAt(new string(' ', boxModel.Columns), location, boxModel.ZIndex, color, color);
        }

        public static void Clear(IBoxModel boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            var color = ConsoleColor.Black;
            console.DrawAt(new string(' ', boxModel.Columns), location, int.MinValue, color, color);
        }



    }
}
