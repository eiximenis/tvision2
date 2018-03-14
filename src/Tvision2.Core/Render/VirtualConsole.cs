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

        public bool IsDirty { get; private set; }

        public void NoDirty()
        {
            IsDirty = false;
            _diffs.Clear();
        }

        private readonly List<VirtualConsoleUpdate> _diffs;

        public IEnumerable<VirtualConsoleUpdate> GetDiffs() => _diffs;

        public VirtualConsole()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            _buffer = new ConsoleCharacter[Height * Width];
            InitEmpty();
            _diffs = new List<VirtualConsoleUpdate>(Height * Width);
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
            var dirty = false;
            var charCol = location.Left;
            var charRow = location.Top;
            for (var idx = start; idx < end; idx++)
            {
                var cchar = _buffer[idx];
                if (cchar.ZIndex <= zIndex)
                {
                    // TODO: Compare cchar with values passed if we 
                    // want to avoid to redraw all changed characters to console
                    // even if they are the same as before
                    cchar.Character = text[textIdx];
                    cchar.Background = backColor;
                    cchar.Foreground = foreColor;
                    cchar.ZIndex = zIndex;
                    dirty = true;
                    _diffs.Add(new VirtualConsoleUpdate(cchar, charRow, charCol));
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
            var dirty = false;
            for (var idx = start; idx < end; idx++)
            {
                var cchar = _buffer[idx];
                if (cchar.ZIndex <= zindex)
                {
                    // TODO: Compare cchar with values passed if we 
                    // want to avoid to redraw all changed characters to console
                    // even if they are the same as before
                    cchar.Character = charToCopy.Character;
                    cchar.Background = charToCopy.Background;
                    cchar.Foreground = charToCopy.Foreground;
                    cchar.ZIndex = charToCopy.ZIndex;
                    dirty = true;
                    _diffs.Add(new VirtualConsoleUpdate(cchar, charRow, charCol));
                }
                charCol++;
            }

            IsDirty = dirty;
        }
    }
}
