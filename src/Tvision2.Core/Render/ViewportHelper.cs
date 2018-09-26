using System;

namespace Tvision2.Core.Render
{
    public static class ViewportHelper
    {

        public static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
        public static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;

        public static void DrawStringAt(string text, TvPoint location, int pairIdx, IViewport boxModel, VirtualConsole console)
        {
            var consoleLocation = ViewPointToConsolePoint(location, boxModel.Position);
            var zindex = boxModel.ZIndex;

            if (boxModel.Columns < text.Length)
            {
                text = text.Substring(0, boxModel.Columns);
            }
            console.DrawAt(text, consoleLocation, zindex, pairIdx);
        }

        internal static void DrawChars(char value, int count, TvPoint location, int pairIdx, IViewport boxModel, VirtualConsole console)
        {

            var chars = new ConsoleCharacter[count];
            var zindex = boxModel.ZIndex;
            var cc = new ConsoleCharacter() { Character = value, PairIndex = pairIdx, ZIndex = zindex };
            var pos = ViewPointToConsolePoint(location, boxModel.Position);
            console.CopyCharacter(pos, cc, count);
        }

        public static bool IsConsolePointInside(TvPoint consolePoint, TvPoint viewportPosition)
        {
            var viewPoint = ConsolePointToViewport(consolePoint, viewportPosition);
            return consolePoint.Top >= viewportPosition.Top && consolePoint.Left >= viewportPosition.Left;
        }

        public static void Fill(int pairIdx, IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            for (var rows = 0; rows < boxModel.Rows; rows++)
            {
                console.DrawAt(new string(' ', boxModel.Columns), location + new TvPoint(0, rows), boxModel.ZIndex, pairIdx);
            }
        }

        public static void Clear(IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            console.DrawAt(new string(' ', boxModel.Columns), location, int.MinValue, 0);
        }



    }
}
