using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Events;
using Tvision2.Events.Win32;

namespace Tvision2.ConsoleDriver.Win32
{


    enum Win32MouseEventFlags : uint
    {
        ClickOrUp = 0x0,
        MouseMoved = 0x1,
        DoubleClick = 0x2,
        Mouse_Wheeled = 0x4,
        Mouse_HorWheeled = 0x8
    }

    [Flags]
    enum Win32MouseButtons : uint
    {
        None = 0x0,
        FromLeft1st = 0x1,
        Right = 0x2,
        FromLeft2nd = 0x4,
        FromLeft3rd= 0x8,
        FromLeft4th = 0x10
    }

    static class Win32MouseButtonsExtensions
    {
        public static TvMouseButtonStates ToReleasedButtonStates(this Win32MouseButtons buttons)
        {
            var states = TvMouseButtonStates.None;
            if ((buttons & Win32MouseButtons.FromLeft1st) == Win32MouseButtons.FromLeft1st) 
                states |= TvMouseButtonStates.LeftButtonReleased;
            if ((buttons & Win32MouseButtons.FromLeft2nd) == Win32MouseButtons.FromLeft2nd)
                states |= TvMouseButtonStates.SecondLeftButtonReleased;
            if ((buttons & Win32MouseButtons.FromLeft3rd) == Win32MouseButtons.FromLeft3rd)
                states |= TvMouseButtonStates.ThirdLeftButtonReleased;
            if ((buttons & Win32MouseButtons.Right) == Win32MouseButtons.Right)
                states |= TvMouseButtonStates.RightButtonReleased;
            return states;
        }
        public static TvMouseButtonStates ToPressedButtonStates(this Win32MouseButtons buttons)
        {
            var states = TvMouseButtonStates.None;
            if ((buttons & Win32MouseButtons.FromLeft1st) == Win32MouseButtons.FromLeft1st) 
                states |= TvMouseButtonStates.LeftButtonPressed;
            if ((buttons & Win32MouseButtons.FromLeft2nd) == Win32MouseButtons.FromLeft2nd)
                states |= TvMouseButtonStates.SecondLeftButtonPressed;
            if ((buttons & Win32MouseButtons.FromLeft3rd) == Win32MouseButtons.FromLeft3rd)
                states |= TvMouseButtonStates.ThirdLeftButtonPressed;
            if ((buttons & Win32MouseButtons.Right) == Win32MouseButtons.Right)
                states |= TvMouseButtonStates.RightButtonPressed;
            return states;
        }
        
        public static TvMouseButtonStates ToDoubleClickButtonStates(this Win32MouseButtons buttons)
        {
            var states = TvMouseButtonStates.None;
            if ((buttons & Win32MouseButtons.FromLeft1st) == Win32MouseButtons.FromLeft1st) 
                states |= TvMouseButtonStates.LeftButtonDoubleClicked;
            if ((buttons & Win32MouseButtons.FromLeft2nd) == Win32MouseButtons.FromLeft2nd)
                states |= TvMouseButtonStates.SecondLeftButtonDoubleClicked;
            if ((buttons & Win32MouseButtons.FromLeft3rd) == Win32MouseButtons.FromLeft3rd)
                states |= TvMouseButtonStates.ThirdLeftButtonDoubleClicked;
            if ((buttons & Win32MouseButtons.Right) == Win32MouseButtons.Right)
                states |= TvMouseButtonStates.RightButtonDoubleClicked;
            return states;
        }
        
    }
    
    static class TvConsoleEventsExtensions
    {
        public static TvConsoleEvents AddWin32Events(this TvConsoleEvents events, Span<INPUT_RECORD> buffer, ref Win32MouseButtons buttonsPressed)
        {
            foreach (var record in buffer)
            {
                switch (record.EventType)
                {
                    case ConsoleEventTypes.FOCUS_EVENT:
                        break;
                    case ConsoleEventTypes.KEY_EVENT:
                        if (record.KeyEvent.bKeyDown)
                        {
                            events.Add(new Win32ConsoleKeyboardEvent(record.KeyEvent));
                        }
                        break;
                    case ConsoleEventTypes.MOUSE_EVENT:
                        events.Add(CreateMouseEventFromWin32(in record.MouseEvent, ref buttonsPressed));
                        break;
                    case ConsoleEventTypes.WINDOW_BUFFER_SIZE_EVENT:
                        events.SetWindowEvent(new Win32WindowsEvent(record.WindowBufferSizeEvent));
                        break;
                }
            }

            return events;
        }

        private static TvConsoleMouseEvent CreateMouseEventFromWin32(in MOUSE_EVENT_RECORD recordMouseEvent, ref Win32MouseButtons buttonsPressed)
        {
            var states = TvMouseButtonStates.None;
            var buttons = (Win32MouseButtons) recordMouseEvent.dwButtonState;
            var eventFlags = (Win32MouseEventFlags)recordMouseEvent.dwEventFlags;

            switch (eventFlags)
            {
                case Win32MouseEventFlags.ClickOrUp when buttons == Win32MouseButtons.None:
                {
                    // This means all buttons of the mouse are released.
                    states = buttonsPressed.ToReleasedButtonStates();
                    buttonsPressed = Win32MouseButtons.None;
                    break;
                }
                case Win32MouseEventFlags.ClickOrUp:
                {
                    // If the bit is set then the button is pressed.
                    // If the bit is released then the button is released if and only if it exists in buttonsPressed
                    buttons.ToPressedButtonStates();
                    var both = buttonsPressed & buttons;
                    states |= both.ToReleasedButtonStates();
                    buttonsPressed = buttons;
                    break;
                }
                
                case Win32MouseEventFlags.DoubleClick:
                    states = buttons.ToDoubleClickButtonStates();
                    break;
            }


            var modifiers = recordMouseEvent.dwControlKeyState.GetModifiers();
            if (modifiers.ctrl) states |= TvMouseButtonStates.CtrlKey;
            if (modifiers.alt) states |= TvMouseButtonStates.AltKey;
            if (modifiers.shift) states |= TvMouseButtonStates.ShiftKey;
            var evt = new TvConsoleMouseEvent(recordMouseEvent.dwMousePosition.X,  recordMouseEvent.dwMousePosition.Y, states);
            return evt;
            
        }
    }
}
