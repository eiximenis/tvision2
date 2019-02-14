using System;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Dropdown;
using Tvision2.Controls.Label;
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

            var combo = new TvDropdown(TvDropdown.CreationParametersBuilder(state =>
            {
                state.AddValue(new DropDownValue("1", "One"));
                state.AddValue(new DropDownValue("2", "Two"));
                state.AddValue(new DropDownValue("3", "Three"));
            })
            .UseSkin(_skinManager.CurrentSkin).UseViewport(new Viewport(new TvPoint(10, 10), 10, 5, 0)));
            var button = new TvButton(
                TvButton.CreationParametersBuilder(s => s.Text = "Click Me!")
                .UseSkin(_skinManager.CurrentSkin).UseViewport(new Viewport(new TvPoint(22, 10), 15)));

            button.OnClick.Add(state =>
            {
                ShowDialog(state, combo.State);
                return Task.FromResult(true);
            });
            tui.UI.Add(combo);
            tui.UI.Add(button);
        }

        private Task<bool> ShowDialog(ButtonState btnstate, DropdownState comboState)
        {
            var dialog = _dialogManager.CreateDialog(_layoutManager.ViewportFactory.FullViewport().CreateCentered(20, 5),
                dlg =>
                {
                    var label = new TvLabel(TvLabel
                        .CreationParametersBuilder(s => s.Text = "Hello " + comboState.SelectedValue)
                        .UseSkin(_skinManager.CurrentSkin)
                        .UseViewport(new Viewport(TvPoint.Zero, 18)));
                    dlg.State.UI.Add(label);
                });

            _dialogManager.ShowDialog(dialog);

            return Task.FromResult(true);
        }
    }
}
