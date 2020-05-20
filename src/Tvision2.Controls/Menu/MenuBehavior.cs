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

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext) => false;

        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext) => false;

    }
}