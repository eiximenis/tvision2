using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Menu
{
    public class MenuBarBehavior : KeyboardBehavior<MenuState>
    {

        private readonly TvControlMetadata _owner;
        private readonly TvMenuBarOptions _options;

        public MenuBarBehavior(TvControlMetadata owner, TvMenuBarOptions options)
        {
            _owner = owner;
            _options = options;
        }
        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext)
        {

            var info = evt.AsConsoleKeyInfo();
            if (info.Key == _options.Hotkey)
            {
                _owner.OwnerTree.ReturnFocusToPrevious();
                return false;
            }

            var optidx = 0;
            foreach (var option in updateContext.State.Options)
            {
                if (info.KeyChar == option.Shortcut)
                {
                    updateContext.State.SelectedIndex = optidx;
                    return true;
                }
                optidx++;
            }

            return false;

        }
        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext) => false;

    }
}