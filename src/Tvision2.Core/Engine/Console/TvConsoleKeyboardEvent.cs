using System;
namespace Tvision2.Events
{
    public abstract class TvConsoleKeyboardEvent
    {
        public bool IsKeyDown { get; }

        public int RepeatCount { get; }
        
        public bool IsHandled { get; private set; }

        protected TvConsoleKeyboardEvent(bool isKeyDown, int repeatCount)
        {
            IsKeyDown = isKeyDown;
            RepeatCount = repeatCount;
            IsHandled = false;
        }

        public void Handle() => IsHandled = true;

        public abstract ConsoleKeyInfo AsConsoleKeyInfo();
    }
}