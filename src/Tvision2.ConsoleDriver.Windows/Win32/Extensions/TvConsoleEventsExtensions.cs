using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    
}
