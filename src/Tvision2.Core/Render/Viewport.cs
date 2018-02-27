using System; 
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class Viewport 
    {

        private VirtualConsole _console;
        private readonly IBoxModel _boxModel;
        

        public void AttachConsole(VirtualConsole consoleToAttach)
        {
            _console = consoleToAttach;
        }

        public Viewport(IBoxModel boxModel)
        {
            _boxModel = boxModel;
        }

        public TvPoint ViewPointToConsolePoint(TvPoint viewPoint)
        {
            var pos = _boxModel.Position;
            var top = viewPoint.Top + pos.Top;
            var left = viewPoint.Left + pos.Left;
            return new TvPoint(top, left);
        }

        public TvPoint ConsolePointToViewport(TvPoint consolePoint)
        {
            var pos = _boxModel.Position;
            var top = consolePoint.Top - pos.Top;
            var left = consolePoint.Left - pos.Left;
            return new TvPoint(top, left);
        }

        public void DrawStringAt(string text, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor)
        {

            var maxCharsToDraw = text.Length;
            var clippingMode = _boxModel.Clipping;
            var consoleLocation = ViewPointToConsolePoint(location);

            if ((clippingMode == ClippingMode.Clip || clippingMode == ClippingMode.ExpandVertical) && _boxModel.Columns < maxCharsToDraw)
            {
                maxCharsToDraw = _boxModel.Columns;
            }
            else
            {
                _boxModel.Grow(maxCharsToDraw, _boxModel.Rows);
                Clear(backColor, consoleLocation);
            }
            
            _console.DrawAt(text.Substring(0, maxCharsToDraw), consoleLocation, _boxModel.ZIndex, foreColor, backColor);
        }

        public bool IsConsolePointInside(TvPoint consolePoint)
        {
            var pos = _boxModel.Position;
            var viewPoint = ConsolePointToViewport(consolePoint);
            return consolePoint.Top >= pos.Top && consolePoint.Left >= pos.Left;
        }

        private void Clear(ConsoleColor color, TvPoint location)
        {
            _console.DrawAt(new string(' ', _boxModel.Columns), location, _boxModel.ZIndex, color, color);
        }



    }
}
