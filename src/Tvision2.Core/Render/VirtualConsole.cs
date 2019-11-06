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
            OnlyChar = 1,
            OnlyAttributes = 2,
            CharAndAttr = 3
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

        public void Resize(int height, int width)
        {
            if (Height != height || Width != width)
            {
                Height = height;
                Width = width;
                InitData();
                for (var idx = 0; idx<_dirtyMap.Length; idx++)
                {
                    _dirtyMap[idx] = DirtyStatus.CharAndAttr;
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
            var idx = 0;
            for (var row = 0; row < Height; row++)
            {
                var span = 1;
                var spanning = false;
                var spanCol = 0;
                for (var col = 0; col < Width; col++)
                {
                    var next = col < Width - 1 ? _buffer[idx + 1] : null;
                    var cc = _buffer[idx];
                    if (spanning || _dirtyMap[idx] != DirtyStatus.Clean)
                    {
                        if (cc.Equals(next))
                        {
                            span++;
                            spanning = true;
                        }
                        else
                        {
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


            if (Cursor.ActionPending == VirtualConsoleCursorAction.Show)
            {
                consoleDriver.SetCursorVisibility(isVisible: true);
                Cursor.VisibilityChanged(isVisible: true);
            }

            if (Cursor.MovementPending)
            {
                consoleDriver.SetCursorAt(Cursor.Position.Left, Cursor.Position.Top);
                Cursor.MovementPending = false;
            }
            else if (Cursor.ActionPending == VirtualConsoleCursorAction.Hide)
            {
                consoleDriver.SetCursorVisibility(isVisible: false);
                Cursor.VisibilityChanged(isVisible: false);
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
                    Attributes = new CharacterAttribute(),
                    ZIndex = int.MinValue
                };
            }
            IsDirty = false;
        }

        public void DrawAt(string text, TvPoint location, int zIndex, CharacterAttribute attr)
        {
            var start = location.Left + (Width * location.Top);
            var end = start + Math.Min(text.Length, Width - location.Left);
            var textIdx = 0;
            var dirty = IsDirty;
            var charCol = location.Left;
            var charRow = location.Top;
            for (var idx = start; idx < end; idx++)
            {
                var cchar = _buffer[idx];
                var newchar = text[textIdx];
                if (cchar.ZIndex <= zIndex)
                {
                    var comparison = cchar.CompareContents(newchar, attr);
                    if (comparison != ConsoleCharacterComparison.Equals)
                    {
                        cchar.Character = newchar;
                        cchar.Attributes = attr;
                        cchar.ZIndex = zIndex;
                        dirty = true;
                        _dirtyMap[idx] = (DirtyStatus)comparison;
                    }
                    cchar.ZIndex = zIndex;
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
                    _buffer[idx].Character = ' ';
                    _buffer[idx].Attributes = new CharacterAttribute();
                    _dirtyMap[idx] = DirtyStatus.CharAndAttr;
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
            var charRow = location.Top;
            var dirty = IsDirty;
            for (var idx = start; idx < end; idx++)
            {
                var cchar = _buffer[idx];
                if (cchar.ZIndex <= zindex)

                {
                    var comparison = cchar.CompareContents(charToCopy.Character, charToCopy.Attributes);
                    if (comparison != ConsoleCharacterComparison.Equals)
                    {
                        cchar.Character = charToCopy.Character;
                        cchar.Attributes = charToCopy.Attributes;
                        cchar.ZIndex = charToCopy.ZIndex;
                        dirty = true;
                        _dirtyMap[idx] = (DirtyStatus)comparison;
                    }
                    cchar.ZIndex = zindex;
                }
                charCol++;
            }

            IsDirty = dirty;
        }

    }
}
