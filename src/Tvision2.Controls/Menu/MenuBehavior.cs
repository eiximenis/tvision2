using System.Buffers;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Events;

namespace Tvision2.Controls.Menu
{
    public class MenuBehavior : KeyboardBehavior<MenuState>
    {
        private TvMenu _owner;

        public MenuBehavior(TvMenu owner)
        {
            _owner = owner;
        }

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext)
        {
            var info = evt.AsConsoleKeyInfo();

            if (info.Key == System.ConsoleKey.Escape)
            {
                evt.Handle();
                var parent = updateContext.ComponentLocator.GetParentControl();
                var menubar = parent.Control as TvMenuBar;
                menubar.Close(_owner);
            }
            return false;
        }

        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext) => false;

    }
}