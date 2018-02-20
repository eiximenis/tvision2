using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.Win32;

namespace Tvision2.Events.NCurses
{
    class NCursesConsoleKeyboardEvent : TvConsoleKeyboardEvent
    {
       private readonly int _wch;

        public NCursesConsoleKeyboardEvent(int wch) : base(true,1,(char)wch)
        {
            _wch = wch;
        }

        public override ConsoleKeyInfo AsConsoleKeyInfo() => AsConsoleKeyInfo(Character, _wch);

        public static ConsoleKeyInfo AsConsoleKeyInfo(char character, int wch)
        {
            //var (ctrl, alt, shift) = record.dwControlKeyState.GetModifiers();
            return new ConsoleKeyInfo(character, ConsoleKey.A, false, false, false);

        }
    }
}
