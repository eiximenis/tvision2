using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.Win32;

namespace Tvision2.Events.NCurses
{
    class NCursesConsoleKeyboardEvent : TvConsoleKeyboardEvent
    {
       private readonly int _wch;
       private readonly bool _alt;

        public NCursesConsoleKeyboardEvent(int wch, bool alt, bool isDown) : base(isDown,1,(char)wch)
        {
            _wch = wch;
            _alt = alt;
        }

        public override ConsoleKeyInfo AsConsoleKeyInfo() 
        {
            var ctrl = (_wch & 0x1f) != 0;
            return new ConsoleKeyInfo(Character, ConsoleKey.A, false, _alt, ctrl);
        }


    }
}
