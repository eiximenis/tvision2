using System;
using Tvision2.Core.Colors;

namespace Tvision2.Core.Render
{
    public static class ViewportHelperNoneFlow
    {

        public static TvPoint ViewPointToConsolePoint(TvPoint viewPoint, TvPoint viewportPosition) => viewPoint + viewportPosition;
        public static TvPoint ConsolePointToViewport(TvPoint consolePoint, TvPoint viewportPosition) => consolePoint - viewportPosition;

        public static void DrawStringAt(string text, TvPoint location, CharacterAttribute attr, IViewport viewport, VirtualConsole console)
        {
            var consoleLocation = ViewPointToConsolePoint(location, viewport.Position);
            var zindex = viewport.ZIndex;
            var maxcols = (viewport.Bounds.Cols - location.Left);
            var textSpan = maxcols < text.Length ?
                    text.AsSpan(0, maxcols) :
                    text.AsSpan();
            console.DrawAt(textSpan, consoleLocation, (int)zindex, attr);
        }

        internal static void DrawChars(char value, int count, TvPoint location, CharacterAttribute attribute, IViewport viewport, VirtualConsole console)
        {
            var zindex = viewport.ZIndex;
            var cc = new ConsoleCharacter(value, attribute, zindex);
            var pos = ViewPointToConsolePoint(location, viewport.Position);
            var maxcols = (viewport.Bounds.Cols - location.Left);
            console.CopyCharacter(pos, cc, maxcols < count ? maxcols : count);
        }

      

    }
}
