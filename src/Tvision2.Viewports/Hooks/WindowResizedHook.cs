using System.Linq;
using Tvision2.Core.Hooks;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.Viewports.Hooks
{
    public class WindowResizedHook : IEventHook
    {
        private readonly IConsoleDriver _driver;
        public WindowResizedHook(IConsoleDriver driver)
        {
            _driver = driver;
        }

        public void ProcessEvents(ITvConsoleEvents events, HookContext context)
        {
            var evt = events.WindowEvent;
            if (evt != null)
            {
                var cmps = context.ComponentTree.Components.ToList();
                foreach (var cmp in cmps)
                {
                    foreach (var vp in cmp.Viewports.ToList())
                    {
                        if (vp.Value is DynamicViewport dp)
                        {
                            cmp.UpdateViewport(vp.Key, dp.Recalculate());
                        }
                    }
                }

            }
        }
    }
}
