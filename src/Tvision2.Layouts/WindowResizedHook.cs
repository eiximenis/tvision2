using System.Linq;
using Tvision2.Core.Hooks;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.Layouts
{
    public class WindowResizedHook : IEventHook
    {
        private readonly IConsoleDriver _driver;
        private readonly ILayoutManager _layoutManager;
        public WindowResizedHook(IConsoleDriver driver, ILayoutManager layoutManager)
        {
            _driver = driver;
            _layoutManager = layoutManager;
        }

        public void ProcessEvents(TvConsoleEvents events, HookContext context)
        {
            var evt = events.WindowEvents.LastOrDefault();
            if (evt != null)
            {

            }
        }
    }
}
