using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.Win32;

namespace Tvision2.Events.Win32
{
    class Win32ConsoleKeyboardEvent : TvConsoleKeyboardEvent
    {
        private readonly KEY_EVENT_RECORD _record;

        public Win32ConsoleKeyboardEvent(KEY_EVENT_RECORD record) : base(record.bKeyDown, record.wRepeatCount, record.UnicodeChar)
        {
            _record = record;
        }

        public override ConsoleKeyInfo AsConsoleKeyInfo() => AsConsoleKeyInfo(_record);

        public static ConsoleKeyInfo AsConsoleKeyInfo(KEY_EVENT_RECORD record)
        {
            var (ctrl, alt, shift) = record.dwControlKeyState.GetModifiers();
            return new ConsoleKeyInfo(record.UnicodeChar, (ConsoleKey)record.wVirtualKeyCode, shift, alt, ctrl);

        }
    }
}
