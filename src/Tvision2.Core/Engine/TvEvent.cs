using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Engine
{
    public abstract class TvEvent
    {
        public DateTime When { get; }
        public TvEvent() => When = DateTime.Now;
    }

    public class KeyEvent : TvEvent
    {
        public ConsoleKeyInfo KeyInfo { get; }
        public KeyEvent(ConsoleKeyInfo key) => KeyInfo = key;
    }
}
