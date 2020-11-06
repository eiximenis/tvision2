using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Button;
using Tvision2.Controls.Dropdown;
using Tvision2.Controls.Label;
using Tvision2.Core.Render;
using Tvision2.Layouts.Grid;
using Tvision2.Viewports;

namespace Tvision2.ControlsGallery
{
    public static class ControlsFactory
    {
        public static TvGrid CreateGrid(IViewportFactory viewportFactory)
        {
            var grid = new TvGrid(GridState.FromRowsAndColumns(3, 4));
            grid.AsComponent().AddViewport(viewportFactory.FullViewport());
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

        public static TvButton CreateButton()
        {
            var button = new TvButton(
                TvButton.UseParams().WithState(ButtonState.FromText("Click Me!")).Configure(c => c.UseControlName("button").UseViewport(new Viewport(TvPoint.FromXY(22, 10), 15))).Build()); ;

            return button;
        }

        public static TvLabel CreateLabel()
        {
            var lparams = TvLabel.UseParams().WithState(LabelState.FromText("Tvision2 Controls")).Configure(c => c.UseViewport(Viewport.NullViewport)).Build();
            return new TvLabel(lparams);
        }
    }
}
