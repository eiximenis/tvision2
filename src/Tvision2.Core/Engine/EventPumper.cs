using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.ConsoleDriver;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public class EventPumper
    {

        private readonly IConsoleDriver _console;
        public EventPumper(IConsoleDriver console) => _console = console;

        public TvConsoleEvents ReadEvents()
        {
            var events = _console.ReadEvents();
            return events;
        }
    }
}
