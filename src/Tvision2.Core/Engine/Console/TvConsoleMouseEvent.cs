using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Events
{
    public sealed class TvConsoleMouseEvent
    {
        public int X { get; }
        public int Y { get; }
        public bool CtrlPressed { get; }
        public bool AltPressed { get; }
        public bool ShiftPressed { get; }
        public TvMouseButtonStates ButtonStates { get; }
        public bool IsHandled { get; private set; }
        public bool IsDoubleClickEvent { get; }

        public bool IsClickEvent { get; }
        public bool IsPressedEvent { get; }
        public bool IsReleasedEvent { get; }


        public TvConsoleMouseEvent(int x, int y, TvMouseButtonStates buttonStates)
        {
            IsHandled = false;
            X = x;
            Y = y;
            ButtonStates = buttonStates;
            AltPressed = (buttonStates & TvMouseButtonStates.AltKey) == TvMouseButtonStates.AltKey;
            CtrlPressed = (buttonStates & TvMouseButtonStates.CtrlKey) == TvMouseButtonStates.CtrlKey;
            ShiftPressed = (buttonStates & TvMouseButtonStates.ShiftKey) == TvMouseButtonStates.ShiftKey;

            IsDoubleClickEvent = (ButtonStates & TvMouseButtonStates.LeftButtonDoubleClicked) == TvMouseButtonStates.LeftButtonDoubleClicked ||
                   (ButtonStates & TvMouseButtonStates.RightButtonDoubleClicked) == TvMouseButtonStates.RightButtonClicked ||
                   (ButtonStates & TvMouseButtonStates.SecondLeftButtonDoubleClicked) == TvMouseButtonStates.SecondLeftButtonDoubleClicked ||
                   (ButtonStates & TvMouseButtonStates.ThirdLeftButtonDoubleClicked) == TvMouseButtonStates.ThirdLeftButtonDoubleClicked;

            IsClickEvent = (ButtonStates & TvMouseButtonStates.LeftButtonClicked) == TvMouseButtonStates.LeftButtonClicked ||
                   (ButtonStates & TvMouseButtonStates.RightButtonClicked) == TvMouseButtonStates.RightButtonClicked ||
                   (ButtonStates & TvMouseButtonStates.SecondLeftButtonClicked) == TvMouseButtonStates.SecondLeftButtonClicked ||
                   (ButtonStates & TvMouseButtonStates.ThirdLeftButtonClicked) == TvMouseButtonStates.ThirdLeftButtonClicked;

            IsPressedEvent = (ButtonStates & TvMouseButtonStates.LeftButtonPressed) == TvMouseButtonStates.LeftButtonPressed ||
                   (ButtonStates & TvMouseButtonStates.RightButtonPressed) == TvMouseButtonStates.RightButtonPressed ||
                   (ButtonStates & TvMouseButtonStates.SecondLeftButtonPressed) == TvMouseButtonStates.SecondLeftButtonPressed ||
                   (ButtonStates & TvMouseButtonStates.ThirdLeftButtonPressed) == TvMouseButtonStates.ThirdLeftButtonPressed;

            IsReleasedEvent = (ButtonStates & TvMouseButtonStates.LeftButtonReleased) == TvMouseButtonStates.LeftButtonReleased ||
                   (ButtonStates & TvMouseButtonStates.RightButtonReleased) == TvMouseButtonStates.RightButtonReleased ||
                   (ButtonStates & TvMouseButtonStates.SecondLeftButtonReleased) == TvMouseButtonStates.SecondLeftButtonReleased ||
                   (ButtonStates & TvMouseButtonStates.ThirdLeftButtonReleased) == TvMouseButtonStates.ThirdLeftButtonReleased;

        }

        public void Handle() => IsHandled = true;
    }

    [Flags]
    public enum TvMouseButtonStates
    {
        None = 0x0,
        LeftButtonReleased = 0x1,
        LeftButtonPressed = 0x2,
        LeftButtonClicked = 0x4,
        LeftButtonDoubleClicked = 0x8,
        LeftButtonTripleClicked = 0x10,
        RightButtonReleased = 0x40,
        RightButtonPressed = 0x80,
        RightButtonClicked = 0x100,
        RightButtonDoubleClicked = 0x200,
        RightButtonTripleClicked = 0x400,
        SecondLeftButtonReleased = 0x1000,
        SecondLeftButtonPressed = 0x2000,
        SecondLeftButtonClicked = 0x4000,
        SecondLeftButtonDoubleClicked = 0x8000,
        SecondLeftButtonTripleClicked = 0x10000,
        ThirdLeftButtonReleased = 0x40000,
        ThirdLeftButtonPressed = 0x8000,
        ThirdLeftButtonClicked = 0x100000,
        ThirdLeftButtonDoubleClicked = 0x200000,
        ThirdLeftButtonTripleClick = 0x400000,
        CtrlKey = 0x1000000,
        ShiftKey = 0x2000000,
        AltKey = 0x4000000
    }



}
