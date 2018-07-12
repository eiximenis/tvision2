using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Checkbox;
using Tvision2.Controls.Label;
using Tvision2.Controls.Styles;
using Tvision2.Controls.Textbox;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts;
using Tvision2.Layouts.Grid;

namespace Tvision2.MidnightCommander
{
    public class Startup : ITvisionAppStartup
    {
        private readonly ISkinManager _skinManager;
        private readonly ILayoutManager _layoutManager;
        public Startup(ISkinManager skinManager, ILayoutManager layoutManager)
        {
            _skinManager = skinManager;
            _layoutManager = layoutManager;
        }
        Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {
            var skin = _skinManager.CurrentSkin;
            var vpf = _layoutManager.ViewportFactory;
            var grid = new TvGrid(tui.UI, new GridState(1, 2));
            grid.AsComponent().AddViewport(vpf.FullViewport().Grow(0, -2));
            var textbox = new TvTextbox(skin, vpf.BottomViewport(2), new TextboxState());

            var left = new TvLabel(skin, new Viewport(new TvPoint(0, 0), 10, 1), new LabelState() { Text = "Left" });
            var right = new TvLabel(skin, new Viewport(new TvPoint(0, 0), 10, 1), new LabelState() { Text = "Right" });
            grid.Use(0,0).Add(left.AsComponent());
            grid.Use(0, 1).Add(textbox);
            tui.UI.Add(grid);
            return Task.CompletedTask;
        }
    }
}
