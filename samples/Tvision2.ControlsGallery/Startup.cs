using System;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Dropdown;
using Tvision2.Controls.Label;
using Tvision2.Controls.Styles;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Dialogs;
using Tvision2.Layouts;
using Tvision2.Layouts.Grid;
using Tvision2.Styles;
using Tvision2.Viewports;

namespace Tvision2.ControlsGallery
{
    public class Startup : ITvisionAppStartup
    {

        private readonly ILayoutManager _layoutManager;
        private readonly IDialogManager _dialogManager;
        private readonly IViewportFactory _vpFactory;
        private readonly ISkinManager _skinManager;

        public Startup(ILayoutManager layoutManager, IDialogManager dialogManager, IViewportFactory vpFactory, ISkinManager skinManager)
        {
            _layoutManager = layoutManager;
            _dialogManager = dialogManager;
            _vpFactory = vpFactory;
            _skinManager = skinManager;
        }


        async Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {

            _skinManager.ChangeCurrentSkin("Mc");
            var mainGrid = ControlsFactory.CreateGrid(_vpFactory, _skinManager);
            tui.UI.Add(mainGrid);

            /*
            var combo = ControlsFactory.CreateDropDown();
            mainGrid.At(row: 1, col: 0).Add(combo.AsComponent());

            var button = ControlsFactory.CreateButton();

            mainGrid.At(row: 1, col: 1).WithAlignment(ChildAlignment.Fill).Add(button.AsComponent());

            button.OnClick.Add(state =>
            {
                ShowDialog(state, combo.State);
                tui.UI.Remove(combo);
            });

            */

            mainGrid.AtRowCol(row: 0, col: 0).Add(ControlsFactory.CreateButton("Button 1", 1, _skinManager));
            var btn2 = ControlsFactory.CreateButton("Button 2", 2, _skinManager);
            mainGrid.AtRowCol(row: 0, col: 1).WithAlignment(ChildAlignment.StretchHorizontal).Add(btn2);
            btn2.OnClick.Add(CreateDialogForButton);
            mainGrid.AtRowCol(row: 1, col: 0).WithAlignment(ChildAlignment.StretchVertical).Add(ControlsFactory.CreateButton("Button 3", 3, _skinManager));
            mainGrid.AtRowCol(row: 1, col: 1).WithAlignment(ChildAlignment.Fill).Add(ControlsFactory.CreateButton("Button 4", 4, _skinManager));
            mainGrid.AtRowCol(row: 2, col: 0).Add(ControlsFactory.CreateDropDown(_skinManager));
        }

        private ActionResult CreateDialogForButton(ButtonState st)
        {
            var dialog = _dialogManager.CreateDialog(new Viewport(TvPoint.FromXY(10, 10), TvBounds.FromRowsAndCols(8, 30), Layer.Standard),
                dlg => dlg.Add(ControlsFactory.CreateLabel($"{st.Text} pressed")));

            _dialogManager.ShowDialog(dialog);

            return ActionResult.Continue;
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
