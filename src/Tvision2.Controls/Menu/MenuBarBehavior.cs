﻿using System;
using Tvision2.Controls.Behavior;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Menu
{
    public class MenuBarBehavior : KeyboardBehavior<MenuState>
    {

        
        private readonly ITvMenuBarOptions _options;

        [OwnerControl]
        public TvMenuBar Owner { get; set; }

        public MenuBarBehavior(ITvMenuBarOptions options)
        {
            _options = options;
        }
        protected override bool OnKeyPress(TvConsoleKeyboardEvent evt, BehaviorContext<MenuState> updateContext)
        {

            var info = evt.AsConsoleKeyInfo();
            
            if (info.Key == ConsoleKey.Escape)
            {
                Owner.CloseCurrentMenu();
                evt.Handle();
                return true;
            }
            
            else if (info.Key == _options.SelectKey)
            {
                Owner.EnableMenu(updateContext.State);
                evt.Handle();
                return true;
            }
            else if (info.Key == _options.Hotkey)
            {
                Owner.Metadata.ReturnFocusToPrevious();
                evt.Handle();
                return false;
            }
            else if (info.Key == ConsoleKey.RightArrow)
            {
                updateContext.State.SelectedIndex++;
                Owner.UpdateCurrentMenu();
                evt.Handle();
                return true;
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {
                updateContext.State.SelectedIndex--;
                Owner.UpdateCurrentMenu();
                evt.Handle();
                return true;
            }

            var optidx = 0;
            foreach (var option in updateContext.State.Entries)
            {
                if (info.KeyChar == option.Shortcut)
                {
                    updateContext.State.SelectedIndex = optidx;
                    evt.Handle();
                    return true;
                }
                optidx++;
            }

            return false;

        }
        

    }
}