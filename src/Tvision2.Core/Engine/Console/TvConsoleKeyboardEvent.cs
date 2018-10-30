using System;
namespace Tvision2.Events
{
    public abstract class TvConsoleKeyboardEvent
    {
        public bool IsKeyDown { get; }

        public int RepeatCount { get; }

        public char Character { get; }

        public bool IsHandled { get; private set; }

        protected TvConsoleKeyboardEvent(bool isKeyDown, int repeatCount, char character)
        {
            IsKeyDown = isKeyDown;
            RepeatCount = repeatCount;
            Character = character;
            IsHandled = false;
        }

        public void Handle() => IsHandled = true;

        public abstract ConsoleKeyInfo AsConsoleKeyInfo();
    }
}