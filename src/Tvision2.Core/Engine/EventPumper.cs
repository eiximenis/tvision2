using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Core.Engine
{
    public class EventPumper
    {


        public EventPumper()
        {

        }

        public TvEventsCollection ReadEvents()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                var events = new TvEventsCollection();
                events.AddEvent(new KeyEvent(key));
                return events;
            }
            return null;
        }
    }
}
