using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Events
{
    public abstract class TvConsoleMouseEvent
    {
        public int X { get; }
        public int Y { get; }
        public bool CtrlPressed { get; protected set; }
        public bool AltPressed { get; protected set; }
        public bool ShiftPressed { get; protected set; }

        public TvConsoleMouseEventType EventType { get; }

        protected TvConsoleMouseEvent(int x, int y, TvConsoleMouseEventType evtType)
        {
            X = x;
            Y = y;
            EventType = evtType;
        }

        public abstract bool HasButtonPressed(TvMouseButton buttonToCheck);

    }

    public enum TvMouseButton : uint
    {
        LeftButton = 0x0001,
        RightButton = 0x0002,
        SecondLeftButton = 0x0004,
        ThirdLeftButton = 0x0008,
        FourthLeftButton = 0x0010
    }


}
