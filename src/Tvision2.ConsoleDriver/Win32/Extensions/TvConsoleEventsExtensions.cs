using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Events;
using Tvision2.Events.Win32;

namespace Tvision2.ConsoleDriver.Win32
{
    static class TvConsoleEventsExtensions
    {
        public static TvConsoleEvents Add(this TvConsoleEvents events, INPUT_RECORD[] buffer)
        {
            foreach (var record in buffer)
            {
                switch (record.EventType)
                {
                    case ConsoleEventTypes.FOCUS_EVENT:
                        break;
                    case ConsoleEventTypes.KEY_EVENT:
                        events.Add(new Win32ConsoleKeyboardEvent(record.KeyEvent));
                        break;
                    case ConsoleEventTypes.MENU_EVENT:
                        break;
                    case ConsoleEventTypes.MOUSE_EVENT:
                        events.Add(new Win32ConsoleMouseEvent(record.MouseEvent));
                        break;
                    case ConsoleEventTypes.WINDOW_BUFFER_SIZE_EVENT:
                        break;
                }
            }

            return events;
        }
    }
}
