using System;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Controls.Menu
{
    public class MenuBarBehavior : KeyboardBehavior<MenuState>
    {

        private readonly TvMenuBar _owner;
        private readonly TvMenuBarOptions _options;

        public MenuBarBehavior(TvMenuBar owner, TvMenuBarOptions options)
        {
            _owner = owner;
            _options = options;
        }
        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext)
        {

            var info = evt.AsConsoleKeyInfo();

            if (info.Key == _options.SelectKey)
            {
                _owner.EnableMenu(updateContext.State);
                return true;
            }

            if (info.Key == _options.Hotkey)
            {
                _owner.Metadata.OwnerTree.ReturnFocusToPrevious();
                return false;
            }

            var optidx = 0;
            foreach (var option in updateContext.State.Entries)
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