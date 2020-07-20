using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Tvision2.Events
{
    public class TvConsoleEvents : ITvConsoleEvents
    {

        private static readonly TvConsoleEventsEmpty _empty;

        static TvConsoleEvents()
        {
            _empty = new TvConsoleEventsEmpty();
        }

        public static ITvConsoleEvents Empty { get { return _empty; } }
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

        public void Clear()
        {
            _keyboardEvents.Clear();
            _mouseEvents.Clear();
            WindowEvent = null;
        }

        public TvConsoleKeyboardEvent AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool> filter, bool autoHandle)
        {
            var evt = _keyboardEvents.Where(x => !x.IsHandled).FirstOrDefault(filter);
            if (autoHandle)
            {
                evt?.Handle();
            }
            return evt;
        }

        public void SetWindowEvent(TvWindowEvent @event) => WindowEvent = @event;
        public void Add(TvConsoleKeyboardEvent @event) => _keyboardEvents.Add(@event);
        public void Add(TvConsoleMouseEvent @event) => _mouseEvents.Add(@event);

    }

    class TvConsoleEventsEmpty : ITvConsoleEvents
    {
        public bool HasEvents => false;

        public bool HasKeyboardEvents => false;

        public bool HasWindowEvent => false;

        public IEnumerable<TvConsoleKeyboardEvent> KeyboardEvents => Enumerable.Empty<TvConsoleKeyboardEvent>();

        public IEnumerable<TvConsoleMouseEvent> MouseEvents => Enumerable.Empty<TvConsoleMouseEvent>();

        public TvWindowEvent WindowEvent => null;

        public TvConsoleKeyboardEvent AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool> filter, bool autoHandle) => null;

        public void Add(TvConsoleKeyboardEvent @event)
        {
            throw new InvalidOperationException("Unsupported on the TvConsoleEvents.Empty instance");
        }

        public void Add(TvConsoleMouseEvent @event)
        {
            throw new InvalidOperationException("Unsupported on the TvConsoleEvents.Empty instance");
        }

        public void SetWindowEvent(TvWindowEvent @event)
        {
            throw new InvalidOperationException("Unsupported on the TvConsoleEvents.Empty instance");
        }
    }
}
