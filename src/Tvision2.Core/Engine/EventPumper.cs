using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvConsole;


namespace Tvision2.Core.Engine
{
    public class EventPumper
    {


        public EventPumper()
        {

        }

        public TvConsoleEvents ReadEvents()
        {
            var events = TvConsole.TvConsole.Instance.ReadEvents();
            return events;
        }
    }
}
