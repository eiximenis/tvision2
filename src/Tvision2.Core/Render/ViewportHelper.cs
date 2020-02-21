using System;
using Tvision2.Core.Colors;

namespace Tvision2.Core.Render
{
    public static class ViewportHelper
    {

        public static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
        public static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;

        public static void DrawStringAt(string text, TvPoint location, CharacterAttribute attr, IViewport viewport, VirtualConsole console)
        {
            var consoleLocation = ViewPointToConsolePoint(location, viewport.Position);
            var zindex = viewport.ZIndex;
            var textSpan = viewport.Bounds.Cols < text.Length ?
                    text.AsSpan(0, viewport.Bounds.Cols) :
                    text.AsSpan();
            console.DrawAt(textSpan, consoleLocation, (int)zindex, attr);
        }

        internal static void DrawChars(char value, int count, TvPoint location, CharacterAttribute attribute, IViewport viewport, VirtualConsole console)
        {
            var zindex = viewport.ZIndex;
            var cc = new ConsoleCharacter(value, attribute, zindex);
            var pos = ViewPointToConsolePoint(location, viewport.Position);
            console.CopyCharacter(pos, cc, count);
        }

        public static bool IsConsolePointInside(TvPoint consolePoint, TvPoint viewportPosition)
        {
            return consolePoint.Top >= viewportPosition.Top && consolePoint.Left >= viewportPosition.Left;
        }

        public static void Fill(CharacterAttribute attr, IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(TvPoint.FromXY(0, 0), boxModel.Position);
            var vpRows = boxModel.Bounds.Rows;
            for (var rows = 0; rows < vpRows; rows++)
            {
                console.DrawAt(new string(' ', boxModel.Bounds.Cols), location +  TvPoint.FromXY(0, rows), (int)boxModel.ZIndex, attr);
            }
        }

        public static void Clear(IViewport boxModel, VirtualConsole console)
        {
            var location = ViewPointToConsolePoint(TvPoint.FromXY(0, 0), boxModel.Position);
            console.DrawAt(new string(' ', boxModel.Bounds.Cols), location, int.MinValue, new CharacterAttribute());
        }



    }
}
