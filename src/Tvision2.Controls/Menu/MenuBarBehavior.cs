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

            if (info.Key == _options.SelectKey)
            {
                EnableMenu(updateContext.State);
                return true;
            }

            if (info.Key == _options.Hotkey)
            {
                _owner.OwnerTree.ReturnFocusToPrevious();
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

        private void EnableMenu(MenuState state)
        {
            /*
            var menu = new TvMenu(TvMenu.CreationParametersBuilder(new[] { "patata", "patata2" })
                .UseViewport(new Viewport(new TvPoint(10, 10), 20))
                    // Todo: Must find some skinresolver here :(
                .UseSkin();
            _owner.Control.AsComponent().Metadata.Engine.UI.Add(menu);
            */
        }

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext) => false;

    }
}