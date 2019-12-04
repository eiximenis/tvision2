using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public class EventPumper
    {

        private readonly IConsoleDriver _console;
        public EventPumper(IConsoleDriver console) => _console = console;

        public ITvConsoleEvents ReadEvents()
        {
            var events = _console.ReadEvents();
            return events;
        }
    }
}
