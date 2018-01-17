using System; 
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class Viewport 
    {
        public TvPoint TopLeft { get; }
        public int Columns { get; }
        public int Rows { get; }

        public int ZIndex { get; }

        private VirtualConsole _console;

        public void AttachConsole(VirtualConsole consoleToAttach)
        {
            _console = consoleToAttach;
        }

        public Viewport(TvPoint topleft, int cols, int rows, int zIndex)
        {
            TopLeft = topleft;
            Columns = cols;
            Rows = rows;
            ZIndex = zIndex;
        }

        public TvPoint ViewPointToConsolePoint(TvPoint viewPoint)
        {
            var top = viewPoint.Top + TopLeft.Top;
            var left = viewPoint.Left + TopLeft.Left;
            return new TvPoint(top, left);
        }

        public TvPoint ConsolePointToViewport(TvPoint consolePoint)
        {
            var top = consolePoint.Top - TopLeft.Top;
            var left = consolePoint.Left - TopLeft.Left;
            return new TvPoint(top, left);
        }

        public void DrawAt(string text, int left, int top, ConsoleColor foreColor, ConsoleColor backColor)
        {
            _console.DrawAt(text, left, top, ZIndex, foreColor, backColor);
        }

        public bool IsConsolePointInside(TvPoint consolePoint)
        {
            var viewPoint = ConsolePointToViewport(consolePoint);
            return consolePoint.Top >= TopLeft.Top && consolePoint.Left >= TopLeft.Left;
        }


    }
}
