using System;

namespace Tvision2.Core.Render
{
    public static class ViewportHelper
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

        public static void DrawStringAt(string text, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor, IViewport boxModel, VirtualConsole console)
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

        internal static void DrawChars(char value, int count, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor, IViewport boxModel, VirtualConsole console)
        {

            var chars = new ConsoleCharacter[count];
            var zindex = boxModel.ZIndex;
            var cc = new ConsoleCharacter() { Character = value, Background = backColor, Foreground = foreColor, ZIndex = zindex };
            var pos = ViewPointToConsolePoint(location, boxModel.Position);
            console.CopyCharacter(pos, cc, count);
        }

        public static bool IsConsolePointInside(TvPoint consolePoint, TvPoint viewportPosition)
        {
            var viewPoint = ConsolePointToViewport(consolePoint, viewportPosition);
            return consolePoint.Top >= viewportPosition.Top && consolePoint.Left >= viewportPosition.Left;
        }

        public static void Fill(ConsoleColor color, IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            for (var rows = 0; rows < boxModel.Rows; rows++)
            {
                console.DrawAt(new string(' ', boxModel.Columns), location + new TvPoint(rows, 0), boxModel.ZIndex, color, color);
            }
        }

        public static void Clear(IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            var color = ConsoleColor.Black;
            console.DrawAt(new string(' ', boxModel.Columns), location, int.MinValue, color, color);
        }



    }
}
