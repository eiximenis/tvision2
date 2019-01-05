﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Dropdown
{
    public class DropDownBehavior : KeyboardBehavior<DropdownState>
    {

        private readonly TvDropdown _owner;
        public DropDownBehavior(TvDropdown owner)
        {
            _owner = owner;
        }

        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<DropdownState> updateContext) => true;

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<DropdownState> updateContext)
        {
            var info = evt.AsConsoleKeyInfo();
            if (info.Key == ConsoleKey.Enter)
            {
                _owner.DisplayList();
                return true;
            }

            return false;
        }
    }
}
