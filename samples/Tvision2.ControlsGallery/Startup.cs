using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls.Dropdown;
using Tvision2.Controls.Styles;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Dialogs;
using Tvision2.Layouts;

namespace Tvision2.ControlsGallery
{
    public class Startup : ITvisionAppStartup
    {

        private readonly ISkinManager _skinManager;
        private readonly ILayoutManager _layoutManager;
        private readonly IDialogManager _dialogManager;

        public Startup(ISkinManager skinManager, ILayoutManager layoutManager, IDialogManager dialogManager)
        {
            _skinManager = skinManager;
            _layoutManager = layoutManager;
            _dialogManager = dialogManager;
        }


        async Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {

            var state = new DropdownState();
            state.AddValue(new DropDownValue("1", "One"));
            state.AddValue(new DropDownValue("2", "Two"));
            state.AddValue(new DropDownValue("3", "Three"));
            var combo = new TvDropdown(_skinManager.CurrentSkin, new Viewport(new TvPoint(10, 10), 10, 5, 0), state, tui);
            tui.UI.Add(combo);
        }
    }
}
