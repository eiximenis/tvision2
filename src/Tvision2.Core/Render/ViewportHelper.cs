using System;
using Tvision2.Core.Colors;

namespace Tvision2.Core.Render
{
    public static class ViewportHelper
    {

        public static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
        public static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;

        public static void DrawStringAt(string text, TvPoint location, CharacterAttribute attr, IViewport boxModel, VirtualConsole console)
        {
            var consoleLocation = ViewPointToConsolePoint(location, boxModel.Position);
            var zindex = boxModel.ZIndex;

            if (boxModel.Columns < text.Length)
            {
                text = text.Substring(0, boxModel.Columns);
            }
            console.DrawAt(text, consoleLocation, zindex, attr);
        }

        internal static void DrawChars(char value, int count, TvPoint location, CharacterAttribute attribute, IViewport boxModel, VirtualConsole console)
        {

            var chars = new ConsoleCharacter[count];
            var zindex = boxModel.ZIndex;
            var cc = new ConsoleCharacter() { Character = value,  Attributes = attribute, ZIndex = zindex };
            var pos = ViewPointToConsolePoint(location, boxModel.Position);
            console.CopyCharacter(pos, cc, count);
        }

        public static bool IsConsolePointInside(TvPoint consolePoint, TvPoint viewportPosition)
        {
            var viewPoint = ConsolePointToViewport(consolePoint, viewportPosition);
            return consolePoint.Top >= viewportPosition.Top && consolePoint.Left >= viewportPosition.Left;
        }

        public static void Fill(CharacterAttribute attr, IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            for (var rows = 0; rows < boxModel.Rows; rows++)
            {
                console.DrawAt(new string(' ', boxModel.Columns), location + new TvPoint(0, rows), boxModel.ZIndex, attr);
            }
        }

        public static void Clear(IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(new TvPoint(0, 0), boxModel.Position);
            console.DrawAt(new string(' ', boxModel.Columns), location, int.MinValue, new CharacterAttribute());
        }



    }
}
