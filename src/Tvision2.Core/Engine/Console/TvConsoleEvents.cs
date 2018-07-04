﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Events
{
    public class TvConsoleEvents
    {
        private readonly List<TvConsoleKeyboardEvent> _keyboardEvents;
        private readonly List<TvConsoleMouseEvent> _mouseEvents;
        public static TvConsoleEvents Empty { get; } = new TvConsoleEvents();
        public IEnumerable<TvConsoleKeyboardEvent> KeyboardEvents => _keyboardEvents;
        public IEnumerable<TvConsoleMouseEvent> MouseEvents => _mouseEvents;

        public bool HasEvents => _keyboardEvents.Any() || _mouseEvents.Any();

        public bool HasKeyboardEvents => _keyboardEvents.Any();

        public TvConsoleEvents()
        {
            _keyboardEvents = new List<TvConsoleKeyboardEvent>();
            _mouseEvents = new List<TvConsoleMouseEvent>();
        }

        public TvConsoleKeyboardEvent AcquireFirstKeyboard(Func<TvConsoleKeyboardEvent, bool> filter)
        {
            var evt = _keyboardEvents.FirstOrDefault(filter);
            if (evt != null)
            {
                _keyboardEvents.Remove(evt);
            }
            return evt;
        }

        public void Add(TvConsoleKeyboardEvent @event) => _keyboardEvents.Add(@event);
        public void Add(TvConsoleMouseEvent @event) => _mouseEvents.Add(@event);

    }
}