using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks.Sources;
using Tvision2.Core.Hooks;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Controls.Hooks
{
    public class MouseChangeFocusEventHook : IEventHook
    {
        private readonly IControlsTree _ctree;

        public MouseChangeFocusEventHook(IControlsTree ctree) => _ctree = ctree;

        public void ProcessEvents(ITvConsoleEvents events, HookContext context)
        {
            /*
            foreach (var evt in events.MouseEvents.Where(me => me.ButtonStates != TvMouseButtonStates.None))
            {
                Debug.WriteLine($"Mouse Evt. States: {evt.ButtonStates} X: {evt.X} Y: {evt.Y}. Cttl: {evt.CtrlPressed} Alt: {evt.AltPressed} Shift: {evt.ShiftPressed}");
            }
            */

            var b1down = events.MouseLeftButtonDown;
            if (b1down != null)
            {
                var pos = TvPoint.FromXY(b1down.X, b1down.Y);
                var selectedControl = _ctree.LocateControlAt(pos);
                if (selectedControl != null)
                {
                    _ctree.Focus(selectedControl);
                }
            }
        }
    }
}