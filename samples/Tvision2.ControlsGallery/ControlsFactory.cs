using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tvision2.Controls.Button;
using Tvision2.Controls.Dropdown;
using Tvision2.Controls.Label;
using Tvision2.Core.Render;
using Tvision2.Layouts;
using Tvision2.Layouts.Grid;
using Tvision2.Styles;
using Tvision2.Viewports;

namespace Tvision2.ControlsGallery
{
    public static class ControlsFactory
    {
        public static TvGrid CreateGrid(IViewportFactory viewportFactory)
        {
            var grid = TvGrid.With().Rows(2).Columns(2).Name("MainGrid")
                .Viewport(new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(8, 50)))
                .WithOptions(opt => opt.UseBorder(BorderValue.FromHorizontalAndVertical(BorderType.Double, BorderType.Single)))
                .Create();

            return grid;
        }

        public static TvDropdown CreateDropDown()
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
            return combo;
        }

        public static TvButton CreateButton(string text, int idx)
        {
            var button = new TvButton(
                TvButton.UseParams().WithState(ButtonState.FromText(text)).Configure(c => c.UseControlName($"button_{idx}")).Build());

            return button;
        }

        public static TvLabel CreateLabel()
        {
            var lparams = TvLabel.UseParams().WithState(LabelState.FromText("I am just a label")).Build();
            return new TvLabel(lparams);
        }
    }
}
