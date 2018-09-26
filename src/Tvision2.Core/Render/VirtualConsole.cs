using System;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Render
{


    public class VirtualConsole
    {
        private readonly ConsoleCharacter[] _buffer;
        private readonly bool[] _dirtyMap;
        public VirtualConsoleUpdateActions UpdateActions { get; }
        public int Width { get; }
        public int Height { get; }

        public bool IsDirty { get; private set; }

        public VirtualConsoleCursor Cursor { get; }

        public VirtualConsole()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            _buffer = new ConsoleCharacter[Height * Width];
            _dirtyMap = new bool[Height * Width];
            InitEmpty();
            Cursor = new VirtualConsoleCursor();
        }

        public void Flush(IConsoleDriver consoleDriver)
        {
            var idx = 0;
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    if (_dirtyMap[idx])
                    {
                        var cc = _buffer[idx];
                        consoleDriver.WriteCharacterAt(col, row, cc.Character, cc.PairIndex);
                        _dirtyMap[idx] = false;
                    }
                    idx++;
                }
            }

            if (Cursor.MovementPending)
            {
                consoleDriver.SetCursorAt(Cursor.Position.Left, Cursor.Position.Top);
                Cursor.MovementPending = false;
            }


            IsDirty = false;
        }


        private void InitEmpty()
        {
            for (var idx = 0; idx < _buffer.Length; idx++)
            {
                _buffer[idx] = new ConsoleCharacter()
                {
                    Character = ' ',
                    PairIndex = 0,
                    ZIndex = int.MinValue
                };
            }
            IsDirty = false;
        }

        public void DrawAt(string text, TvPoint location, int zIndex, int pairIdx)
        {
            var start = location.Left + (Width * location.Top);
            var end = start + text.Length;
            var textIdx = 0;
            var dirty = IsDirty;
            var charCol = location.Left;
            var charRow = location.Top;
            for (var idx = start; idx < end; idx++)
            {
                var cchar = _buffer[idx];
                var newchar = text[textIdx];
                if (cchar.ZIndex <= zIndex && !cchar.Equals(newchar, pairIdx, zIndex))
                {
                    cchar.Character = newchar;
                    cchar.PairIndex = pairIdx;
                    cchar.ZIndex = zIndex;
                    dirty = true;
                    _dirtyMap[idx] = true;
                }
                textIdx++;
                charCol++;
            }

            IsDirty = dirty;
        }

        public void Clear(IViewport viewport)
        {
            var initcol = viewport.Position.Left;
            var initrow = viewport.Position.Top;
            var maxcol = initcol + viewport.Columns;
            var maxrow = initrow + viewport.Rows;

            for (var row = initrow; row <= maxrow; row++)
            {
                for (var col = initcol; col <= maxcol; col++)
                {
                    var idx = col + (Width * row);
                    _buffer[idx].Character = ' ';
                    _buffer[idx].PairIndex = 0;
                    _buffer[idx].ZIndex = -1;
                }
            }


        }

        public void CopyCharacter(TvPoint location, ConsoleCharacter charToCopy, int count)
        {
            var start = location.Left + (Width * location.Top);
            var zindex = charToCopy.ZIndex;
            var end = start + count;
            var charCol = location.Left;
            var charRow = location.Top;
            var dirty = IsDirty;
            for (var idx = start; idx < end; idx++)
            {
                var cchar = _buffer[idx];
                if (cchar.ZIndex <= zindex && !cchar.Equals(charToCopy))
                {
                    cchar.Character = charToCopy.Character;
                    cchar.PairIndex = charToCopy.PairIndex;
                    cchar.ZIndex = charToCopy.ZIndex;
                    dirty = true;
                    _dirtyMap[idx] = true;
                }
                charCol++;
            }

            IsDirty = dirty;
        }

    }
}
