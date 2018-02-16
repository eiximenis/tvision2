using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Events
{
    public class TvConsoleEvents
    {

        private static TvConsoleEvents _emptyInstance = new TvConsoleEvents();
        private readonly List<TvConsoleKeyboardEvent> _keyboardEvents;
        private readonly List<TvConsoleMouseEvent> _mouseEvents;

        public static TvConsoleEvents Empty => _emptyInstance;
        public IEnumerable<TvConsoleKeyboardEvent> KeyboardEvents => _keyboardEvents;

        public IEnumerable<TvConsoleMouseEvent> MouseEvents => _mouseEvents;

        public bool HasEvents => _keyboardEvents.Any() || _mouseEvents.Any();

        public TvConsoleEvents()
        {
            _keyboardEvents = new List<TvConsoleKeyboardEvent>();
            _mouseEvents = new List<TvConsoleMouseEvent>();
        }

        public void Add(TvConsoleKeyboardEvent @event) => _keyboardEvents.Add(@event);
        public void Add(TvConsoleMouseEvent @event) => _mouseEvents.Add(@event);
    }
}
