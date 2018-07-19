using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Checkbox;
using Tvision2.Controls.Label;
using Tvision2.Controls.List;
using Tvision2.Controls.Styles;
using Tvision2.Controls.Textbox;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;
using Tvision2.MidnightCommander.Stores;
using Tvision2.Statex.Controls;

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
            var textbox = new TvTextbox(skin, null, new TextboxState());


            var f1 = System.IO.Directory.GetFileSystemEntries("C:\\", "*.*");
            var f2 = System.IO.Directory.GetFileSystemEntries("D:\\", "*.*");


            //var left = new TvLabel(skin, new Viewport(new TvPoint(0, 0), 10, 1), new LabelState() { Text = "Left" });
            var left = new TvList(skin, new Viewport(new TvPoint(0, 0), 10, 1), new ListState(Enumerable.Empty<string>()));
            var right = new TvList(skin, new Viewport(new TvPoint(0, 0), 10, 1), new ListState(Enumerable.Empty<string>()));

            var sleft = TvStatexControl.Wrap<TvList, ListState, FileList>(left, opt =>
            {
                opt.UseStore("left");
                opt.UseStatexTransformation((fl, cs) =>
                {
                    int i = 0;
                });
            });
            var sright = TvStatexControl.Wrap<TvList, ListState, FileList>(right, opt =>
            {
                opt.UseStore("right");
            });

            grid.Use(0, 0).Add(left);
            grid.Use(0, 1).Add(right);
            tui.UI.Add(grid);
            var bottom = new TvStackPanel(tui.UI, "BottomContainer");
            bottom.AsComponent().AddViewport(vpf.BottomViewport(2));
            bottom.Children.Add(textbox);
            tui.UI.Add(bottom);
            return Task.CompletedTask;
        }
    }
}
