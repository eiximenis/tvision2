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
using Tvision2.Viewports;

namespace Tvision2.ControlsGallery
{
    public class Startup : ITvisionAppStartup
    {

        private readonly ILayoutManager _layoutManager;
        private readonly IDialogManager _dialogManager;
        private readonly IViewportFactory _vpFactory;

        public Startup(ILayoutManager layoutManager, IDialogManager dialogManager, IViewportFactory vpFactory)
        {
            _layoutManager = layoutManager;
            _dialogManager = dialogManager;
            _vpFactory = vpFactory;
        }


        async Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {

            var mainGrid = ControlsFactory.CreateGrid(_vpFactory);

            var label = ControlsFactory.CreateLabel();
            mainGrid.At(0, 1).Add(label.AsComponent());
                
            var combo = ControlsFactory.CreateDropDown();
            mainGrid.At(1, 0).Add(combo.AsComponent());

            var button = ControlsFactory.CreateButton();

            mainGrid.At(1, 1).Add(button.AsComponent());
            
            button.OnClick.Add(state =>
            {
                ShowDialog(state, combo.State);
                tui.UI.Remove(combo);
            });
            
            tui.UI.Add(mainGrid);
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
