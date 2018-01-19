using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{


    public class VirtualConsole
    {
        private readonly ConsoleCharacter[] _buffer;
        public int Width { get; }
        public int Height { get; }

        public VirtualConsole()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            _buffer = new ConsoleCharacter[Height * Width];
            InitEmpty();
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
        }

        public void DrawAt(string text, TvPoint location, int zIndex, ConsoleColor foreColor, ConsoleColor backColor)
        {
            var start = location.Left + (Width * location.Top);
            var end = start + text.Length;
            var textIdx = 0;
            for (var idx = start; idx < end; idx++)
            {
                if (_buffer[idx].ZIndex <= zIndex)
                {
                    _buffer[idx] = new ConsoleCharacter()
                    {
                        Character = text[textIdx],
                        Background = backColor,
                        Foreground = foreColor
                    };
                }
                textIdx++;
            }
        }


        public VirtualConsoleUpdate[] DiffWith(VirtualConsole other)
        {
            var diff = new VirtualConsoleUpdate[_buffer.Length];
            var count = 0;
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    var idx = col + (Width * row);
                    var currentChar = _buffer[idx];
                    var otherChar = other._buffer[idx];
                    if (!currentChar.Equals(otherChar))
                    {
                        diff[count++] = new VirtualConsoleUpdate(otherChar, currentChar, row, col);
                    }
                }
            }

            if (count > 0)
            {
                var retVal = new VirtualConsoleUpdate[count];
                Array.Copy(diff, retVal, count);
                return retVal;
            }

            return Array.Empty<VirtualConsoleUpdate>();
        }

        public void FillWith(VirtualConsole other)
        {
            Array.Copy(other._buffer, _buffer, _buffer.Length);
        }
    }
}
