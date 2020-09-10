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
using Tvision2.Styles;

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

            var ddParams = TvDropdown.UseParams()
                .WithState(state =>
                {
                    state.AddValue(new DropDownValue("1", "One"));
                    state.AddValue(new DropDownValue("2", "Two"));
                    state.AddValue(new DropDownValue("3", "Three"));
                })
                .Configure(c => c.UseViewport(new Viewport(TvPoint.FromXY(10, 10), TvBounds.FromRowsAndCols(5, 10), Layer.Standard)))
                .Build();


            var combo = new TvDropdown(ddParams);
            var button = new TvButton(
                TvButton.UseParams().WithState(ButtonState.FromText("Click Me!")).Configure(c => c.UseViewport(new Viewport(TvPoint.FromXY(22, 10), 15))).Build());

            button.OnClick.Add(state =>
            {
                ShowDialog(state, combo.State);
                tui.UI.Remove(combo);
            });
            tui.UI.Add(combo);
            tui.UI.Add(button);
        }

        private Task<bool> ShowDialog(ButtonState btnstate, DropdownState comboState)
        {
            var dialog = _dialogManager.CreateDialog(_layoutManager.ViewportFactory.FullViewport().CreateCentered(20, 5),
                dlg =>
                {
                    var labelParams = TvLabel.UseParams()
                        .WithState(LabelState.FromText("Hello " + comboState.SelectedValue))
                        .Configure(c => c.UseViewport(new Viewport(TvPoint.Zero, 18)))
                        .Build();

                    var label = new TvLabel(labelParams);
                    dlg.Add(label);
                });

            _dialogManager.ShowDialog(dialog);

            return Task.FromResult(true);
        }
    }
}
