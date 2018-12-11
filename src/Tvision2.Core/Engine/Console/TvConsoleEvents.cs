using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Events
{
    public class TvConsoleEvents
    {
        public static TvConsoleEvents Empty { get; } = new TvConsoleEvents();
        private readonly List<TvConsoleKeyboardEvent> _keyboardEvents;
        private readonly List<TvConsoleMouseEvent> _mouseEvents;
        public IEnumerable<TvConsoleKeyboardEvent> KeyboardEvents => _keyboardEvents.Where(e => !e.IsHandled);
        public IEnumerable<TvConsoleMouseEvent> MouseEvents => _mouseEvents;
        public TvWindowEvent WindowEvent { get; private set; }

        public bool HasEvents => _keyboardEvents.Any(e => !e.IsHandled) || _mouseEvents.Any() || WindowEvent != null;
        public bool HasKeyboardEvents => _keyboardEvents.Any(e => !e.IsHandled);
        public bool HasWindowEvent => WindowEvent != null;
        public TvConsoleEvents()
        {
            _keyboardEvents = new List<TvConsoleKeyboardEvent>();
            _mouseEvents = new List<TvConsoleMouseEvent>();
            WindowEvent = null;
        }

        public TvConsoleKeyboardEvent AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool> filter)
        {
            var evt = _keyboardEvents.Where(x => !x.IsHandled).FirstOrDefault(filter);
            evt?.Handle();
            return evt;
        }

        public void SetWindowEvent(TvWindowEvent @event) => WindowEvent = @event;
        public void Add(TvConsoleKeyboardEvent @event) => _keyboardEvents.Add(@event);
        public void Add(TvConsoleMouseEvent @event) => _mouseEvents.Add(@event);

    }
}
