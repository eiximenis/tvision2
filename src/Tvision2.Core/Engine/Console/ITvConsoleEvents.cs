using System;
using System.Collections.Generic;

namespace Tvision2.Events
{
    public interface ITvConsoleEvents
    {
        bool HasEvents { get; }
        bool HasKeyboardEvents { get; }
        bool HasWindowEvent { get; }
        IEnumerable<TvConsoleKeyboardEvent> KeyboardEvents { get; }
        IEnumerable<TvConsoleMouseEvent> MouseEvents { get; }
        TvWindowEvent WindowEvent { get; }

        TvConsoleKeyboardEvent AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool> filter);
        void Add(TvConsoleKeyboardEvent @event);
        void Add(TvConsoleMouseEvent @event);
        void SetWindowEvent(TvWindowEvent @event);
    }
}