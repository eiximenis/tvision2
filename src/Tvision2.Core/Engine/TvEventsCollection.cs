using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Core.Engine
{
    public class TvEventsCollection : IEnumerable<TvEvent>
    {

        private readonly List<TvEvent> _events;

        public TvEventsCollection()
        {
            _events = new List<TvEvent>();
        }

        public void AddEvent(TvEvent evt)
        {
            _events.Add(evt);
        }

        public IEnumerator<TvEvent> GetEnumerator()
        {
            foreach (var evt in _events)
            {
                yield return evt;
            }
        }

        public IEnumerable<KeyEvent> KeyPressEvents() => _events.OfType<KeyEvent>();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
