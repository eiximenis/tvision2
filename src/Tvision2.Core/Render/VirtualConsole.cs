using System;
using System.Collections.Generic;
using System.Text;
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
                        consoleDriver.WriteCharacterAt(col, row, cc.Character, cc.Foreground, cc.Background);
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
                    Background = Console.BackgroundColor,
                    Foreground = Console.ForegroundColor,
                    ZIndex = int.MinValue
                };
            }
            IsDirty = false;
        }

        public void DrawAt(string text, TvPoint location, int zIndex, ConsoleColor foreColor, ConsoleColor backColor)
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
                if (cchar.ZIndex <= zIndex && !cchar.Equals(newchar, foreColor, backColor, zIndex))
                {
                    cchar.Value = ConsoleCharacter.ValueFrom(newchar, foreColor, backColor);
                    cchar.ZIndex = zIndex;
                    dirty = true;
                    _dirtyMap[idx] = true;
                }
                textIdx++;
                charCol++;
            }

            IsDirty = dirty;
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
                    cchar.Value = charToCopy.Value;
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
