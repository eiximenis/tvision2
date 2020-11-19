using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Events;
using Tvision2.Events.Win32;

namespace Tvision2.ConsoleDriver.Win32
{
    sealed class Win32EventsProcessor
    {
        private const int CLICK_DIFF = 1500000;
        private Win32MouseButtons _currentButtonsPressed;
        private long _l1tick;
        private long _l2tick;
        private long _l3tick;
        private long _rtick;

        public TvConsoleEvents AddWin32Events(TvConsoleEvents events, Span<INPUT_RECORD> buffer)
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
                        events.Add(CreateMouseEventFromWin32(in record.MouseEvent));
                        break;
                    case ConsoleEventTypes.WINDOW_BUFFER_SIZE_EVENT:
                        events.SetWindowEvent(new Win32WindowsEvent(record.WindowBufferSizeEvent));
                        break;
                }
            }

            return events;
        }

        private TvConsoleMouseEvent CreateMouseEventFromWin32(in MOUSE_EVENT_RECORD recordMouseEvent)
        {
            var states = TvMouseButtonStates.None;
            var buttons = (Win32MouseButtons)recordMouseEvent.dwButtonState;
            var eventFlags = (Win32MouseEventFlags)recordMouseEvent.dwEventFlags;

            switch (eventFlags)
            {
                case Win32MouseEventFlags.ClickOrUp when buttons == Win32MouseButtons.None:
                    {
                        Debug.Write($"*** PREV: {_currentButtonsPressed}  >>> BUTTONS: {buttons}");
                        // This means all buttons of the mouse are released.
                        states = _currentButtonsPressed.ToReleasedButtonStates();
                        states |= GetClickedStatesWithoutPressedButtons();
                        _currentButtonsPressed = Win32MouseButtons.None;
                        Debug.WriteLine($" AFTER: {_currentButtonsPressed}");
                        break;
                    }
                case Win32MouseEventFlags.ClickOrUp:
                    {
                        // If the bit is set then the button is pressed.
                        // If the bit is released then the button is released if and only if it exists in currentButtonsPressed
                        Debug.Write($"+++ PREV: {_currentButtonsPressed}  >>> BUTTONS: {buttons}");
                        states = buttons.ToPressedButtonStates();
                        var both = _currentButtonsPressed & ~buttons;
                        states |= both.ToReleasedButtonStates();
                        states |= GetClickStatesWithPressedButtons(buttons);
                        _currentButtonsPressed = buttons;
                        Debug.WriteLine($" AFTER: {_currentButtonsPressed}");
                        break;
                    }

                case Win32MouseEventFlags.DoubleClick:
                    {
                        // In double click do not report other buttons pressed
                        states = buttons.ToDoubleClickButtonStates();
                        break;
                    }
            }


            var modifiers = recordMouseEvent.dwControlKeyState.GetModifiers();
            if (modifiers.ctrl) states |= TvMouseButtonStates.CtrlKey;
            if (modifiers.alt) states |= TvMouseButtonStates.AltKey;
            if (modifiers.shift) states |= TvMouseButtonStates.ShiftKey;
            var evt = new TvConsoleMouseEvent(recordMouseEvent.dwMousePosition.X, recordMouseEvent.dwMousePosition.Y, states);
            return evt;

        }

        private TvMouseButtonStates GetClickedStatesWithoutPressedButtons()
        {
            var ticks = DateTime.Now.Ticks;
            var clickStates = TvMouseButtonStates.None;

            if (_l1tick != 0 && (ticks - _l1tick) < CLICK_DIFF) clickStates |= TvMouseButtonStates.LeftButtonClicked;
            if (_l2tick != 0 && (ticks - _l2tick) < CLICK_DIFF) clickStates |= TvMouseButtonStates.SecondLeftButtonClicked;
            if (_l3tick != 0 && (ticks - _l3tick) < CLICK_DIFF) clickStates |= TvMouseButtonStates.ThirdLeftButtonClicked;
            if (_rtick != 0 && (ticks - _rtick) < CLICK_DIFF) clickStates |= TvMouseButtonStates.RightButtonClicked;

            _l1tick = _l2tick = _l3tick = _rtick = 0;

            return clickStates;
        }

        private TvMouseButtonStates GetClickStatesWithPressedButtons(Win32MouseButtons buttons)
        {
            // Tick value for each button marks FIRST time button was clicked.
            //   Value is updated ONLY when button is found in clicked and its tick value is 0
            //   Value is reset to 0 when button is released

            // A click event is produced when:
            //  1. We had a button previously pressed (in a prev event)
            //  2. This button is NOW released
            //  3. Time between press<->release is less than specified value

            var ticks = DateTime.Now.Ticks;
            var clickStates = TvMouseButtonStates.None;

            if ((buttons & Win32MouseButtons.FromLeft1st) == Win32MouseButtons.FromLeft1st)
            {
                if (_l1tick == 0) _l1tick = ticks;
            }
            else
            {
                if (_l1tick != 0 && (ticks - _l1tick) < CLICK_DIFF)
                {
                    clickStates |= TvMouseButtonStates.LeftButtonClicked;
                }

                _l1tick = 0;
            }

            if ((buttons & Win32MouseButtons.FromLeft2nd) == Win32MouseButtons.FromLeft2nd)
            {
                if (_l2tick == 0) _l2tick = ticks;
                else
                {
                    if (_l2tick != 0 && (ticks - _l2tick) < CLICK_DIFF)
                    {
                        clickStates |= TvMouseButtonStates.SecondLeftButtonClicked;
                    }
                    _l2tick = 0;
                }
            }

            if ((buttons & Win32MouseButtons.FromLeft3rd) == Win32MouseButtons.FromLeft3rd)
            {
                if (_l3tick == 0) _l3tick = ticks;
                else
                {
                    if (_l3tick != 0 && (ticks - _l3tick) < CLICK_DIFF)
                    {
                        clickStates |= TvMouseButtonStates.SecondLeftButtonClicked;
                    }
                    _l3tick = 0;
                }
            }

            if ((buttons & Win32MouseButtons.Right) == Win32MouseButtons.Right)
            {
                if (_rtick == 0) _rtick = ticks;
                else
                {
                    if (_rtick != 0 && (ticks - _rtick) < CLICK_DIFF)
                    {
                        clickStates |= TvMouseButtonStates.RightButtonClicked;
                    }
                    _rtick = 0;
                }
            }

            return clickStates;
        }
    }
}
