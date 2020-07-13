using System;
using System.Diagnostics;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Render
{


    public class VirtualConsole
    {
        enum DirtyStatus
        {
            Clean = 0,
            Dirty = 1
        }

        private ConsoleCharacter[] _buffer;
        private DirtyStatus[] _dirtyMap;
        public VirtualConsoleUpdateActions UpdateActions { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool IsDirty { get; private set; }

        public VirtualConsoleCursor Cursor { get; private set; }

        public VirtualConsole(int height, int width)
        {
            Width = width;
            Height = height;
            InitData();
        }

        public Span<ConsoleCharacter> GetViewportRow(IViewport viewport, int row)
        {
            var origin = viewport.Position;
            var bounds = viewport.Bounds;
            var consolerow = row + origin.Top; ;
            var startidx = origin.Left + (Width * consolerow);
            return _buffer.AsSpan(startidx, bounds.Cols);
        }

        public void Resize(int height, int width)
        {
            if (Height != height || Width != width)
            {
                Height = height;
                Width = width;
                InitData();
                for (var idx = 0; idx < _dirtyMap.Length; idx++)
                {
                    _dirtyMap[idx] = DirtyStatus.Dirty;
                }
            }
        }

        private void InitData()
        {
            _buffer = new ConsoleCharacter[Height * Width];
            _dirtyMap = new DirtyStatus[Height * Width];
            InitEmpty();
            Cursor = new VirtualConsoleCursor();
        }

        public void Flush(IConsoleDriver consoleDriver)
        {
            Debug.WriteLine("+++ Begin VC Flush");
            consoleDriver.SetCursorVisibility(isVisible: false);
            var idx = 0;
            for (var row = 0; row < Height; row++)
            {
                var span = 1;
                var spanning = false;
                var spanCol = 0;
                for (var col = 0; col < Width; col++)
                {
                    ref var cc = ref _buffer[idx];
                    if (spanning || _dirtyMap[idx] != DirtyStatus.Clean)
                    {
                        var compareWithNext = col < Width - 1;
                        if (compareWithNext && cc.Equals(in _buffer[idx + 1]))
                        {
                            span++;
                            spanning = true;
                        }
                        else
                        {
                            Debug.WriteLine($"Flushing VC with char { cc.Character}");
                            consoleDriver.WriteCharactersAt(spanCol, row, span, cc.Character, cc.Attributes);
                            span = 1;
                            spanning = false;
                            spanCol = col + 1;
                        }
                    }
                    else
                    {
                        spanCol++;
                    }
                    _dirtyMap[idx] = DirtyStatus.Clean;
                    idx++;
                }
            }


            if (Cursor.MovementPending)
            {
                consoleDriver.SetCursorAt(Cursor.Position.Left, Cursor.Position.Top);
                Cursor.MovementPending = false;
            }

            if (Cursor.ActionPending == VirtualConsoleCursorAction.Show)
            {
                Cursor.ChangeVisibility(isVisible: true);
            }
            else if (Cursor.ActionPending == VirtualConsoleCursorAction.Hide)
            {
                Cursor.ChangeVisibility(isVisible: false);
            }

            if (Cursor.Visible)
            {
                consoleDriver.SetCursorAt(Cursor.Position.Left, Cursor.Position.Top);
            }

            IsDirty = false;
        }


        private void InitEmpty()
        {
            for (var idx = 0; idx < _buffer.Length; idx++)
            {
                _buffer[idx] = new ConsoleCharacter(' ', new CharacterAttribute(), Layer.Min);
            }
            IsDirty = false;
        }
        public void DrawAt(string text, TvPoint location, int zIndex, CharacterAttribute attr)
        {
            DrawAt(text.AsSpan(), location, zIndex, attr);
        }

        public void DrawAt(ReadOnlySpan<char> text, TvPoint location, int zIndex, CharacterAttribute attr)
        {
            var start = location.Left + (Width * location.Top);
            var end = start + Math.Min(text.Length, Width - location.Left);
            var textIdx = 0;
            var dirty = IsDirty;
            var charCol = location.Left;
            for (var idx = start; idx < end; idx++)
            {
                ref var cchar = ref _buffer[idx];
                var newchar = new ConsoleCharacter(text[textIdx], attr, zIndex);

                if (cchar.ZIndex <= zIndex && !(cchar.Equals(in newchar)))
                {
                    _buffer[idx] = newchar;
                    dirty = true;
                    _dirtyMap[idx] = DirtyStatus.Dirty;
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
            var maxcol = Math.Min(initcol + viewport.Bounds.Cols, Width - 1);
            var maxrow = Math.Min(initrow + viewport.Bounds.Rows, Height - 1);

            for (var row = initrow; row <= maxrow; row++)
            {
                for (var col = initcol; col <= maxcol; col++)
                {
                    var idx = col + (Width * row);
                    //_buffer[idx] = new ConsoleCharacter('^', new CharacterAttribute(new TvColorPair(TvColor.Red, TvColor.Green)), Layer.Min);
                    _buffer[idx] = _buffer[idx].ToLayer(Layer.Min);
                    _dirtyMap[idx] = DirtyStatus.Dirty;
                }
            }
            IsDirty = true;
        }

        public void CopyCharacter(TvPoint location, ConsoleCharacter charToCopy, int count)
        {
            var start = location.Left + (Width * location.Top);
            var zindex = charToCopy.ZIndex;
            var end = start + Math.Min(count, Width - location.Left);
            var charCol = location.Left;
            var dirty = IsDirty;
            for (var idx = start; idx < end; idx++)
            {
                var cchar = _buffer[idx];
                if (cchar.ZIndex <= zindex && !(cchar.Equals(in charToCopy)))
                {
                    _buffer[idx] = charToCopy;
                    dirty = true;
                    _dirtyMap[idx] = DirtyStatus.Dirty;
                }
                charCol++;
            }

            IsDirty = dirty;
        }

    }
}
