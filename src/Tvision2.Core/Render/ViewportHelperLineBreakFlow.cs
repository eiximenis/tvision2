using System;
using System.Runtime.CompilerServices;
using Tvision2.Core.Colors;

namespace Tvision2.Core.Render
{
    public static class ViewportHelperLineBreakFlow
    {



        public static void DrawStringAt(string text, TvPoint location, CharacterAttribute attr, IViewport viewport, VirtualConsole console)
        {
            var maxcols = (viewport.Bounds.Cols - location.Left);
            var consoleLocation = ViewportHelper.ViewPointToConsolePoint(location, viewport.Position);
            if (text.Length < maxcols)
            {   
                console.DrawAt(text.AsSpan(), consoleLocation, (int)viewport.ZIndex, attr);
            }
            else
            {
                console.DrawAt(text.AsSpan(0, maxcols), consoleLocation, (int)viewport.ZIndex, attr);
                var from = maxcols;
                var line = 1;
                while (from <= text.Length || line >= viewport.Bounds.Rows)
                {
                    var newLinePos = TvPoint.FromXY(consoleLocation.Left - location.Left, consoleLocation.Top + line);
                    console.DrawAt(text.AsSpan(from, viewport.Bounds.Cols), newLinePos, (int)viewport.ZIndex, attr);
                    from += maxcols;
                } 
            }


        }

        internal static void DrawChars(char value, int count, TvPoint location, CharacterAttribute attribute, IViewport viewport, VirtualConsole console)
        {
            var zindex = viewport.ZIndex;
            var cc = new ConsoleCharacter(value, attribute, zindex);
            var consoleLocation = ViewportHelper.ViewPointToConsolePoint(location, viewport.Position);
            var maxchars = (viewport.Bounds.Cols - location.Left);
            if (count <= maxchars)
            {
                console.CopyCharacter(consoleLocation, cc, maxchars < count ? maxchars : count);
            }
            else
            {
                var remaining = count - maxchars;
                var line = 1;
                while(remaining > 0 || line < viewport.Bounds.Rows)
                {
                    var charsInLine = (count - remaining) < viewport.Bounds.Cols ? count-remaining : viewport.Bounds.Cols;
                    console.CopyCharacter(TvPoint.FromXY(consoleLocation.Left - viewport.Position.Left, consoleLocation.Top + line), cc, charsInLine);
                    remaining -= charsInLine;
                    line++;
                } 
            }

        }
    }
}
