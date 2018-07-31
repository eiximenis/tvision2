using System.Linq;
using System.Threading.Tasks;
using Tvision2.Controls.Label;
using Tvision2.Controls.List;
using Tvision2.Controls.Menu;
using Tvision2.Controls.Styles;
using Tvision2.Controls.Textbox;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Dialogs;
using Tvision2.Layouts;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;
using Tvision2.MidnightCommander.Stores;
using Tvision2.Statex;
using Tvision2.Statex.Controls;

namespace Tvision2.MidnightCommander
{
    public class Startup : ITvisionAppStartup
    {
        private readonly ISkinManager _skinManager;
        private readonly ILayoutManager _layoutManager;
        private readonly ITvStoreSelector _storeSelector;
        private readonly IDialogManager _dialogManager;
        public Startup(ISkinManager skinManager, ILayoutManager layoutManager, ITvStoreSelector storeSelector, IDialogManager dialogManager)
        {
            _skinManager = skinManager;
            _layoutManager = layoutManager;
            _storeSelector = storeSelector;
            _dialogManager = dialogManager;
        }
        Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {
            var skin = _skinManager.CurrentSkin;
            var vpf = _layoutManager.ViewportFactory;
            var grid = new TvGrid(tui.UI, new GridState(1, 2));
            grid.AsComponent().AddViewport(vpf.FullViewport().Translate(new TvPoint(0, 1)).Grow(0, -3));
            var textbox = new TvTextbox(skin, null, new TextboxState());

            var left = new TvList(skin, new Viewport(new TvPoint(0, 0), 10, 1, 0), new ListState(Enumerable.Empty<string>()));
            var right = new TvList(skin, new Viewport(new TvPoint(0, 0), 10, 1, 0), new ListState(Enumerable.Empty<string>()));

            var menu = new TvMenuBar(skin, vpf.FullViewport().TakeRows(1, 0), new MenuBarState(new[] { "Left", "Edit", "Command", "Options", "Help", "Right" }));

            //var window = new TvWindow(skin, vpf.FullViewport().CreateCentered(20, 10), new WindowState(tui.UI));
            //var label = new TvLabel(skin, new Viewport(new TvPoint(0, 0), 9, 1, 0), new LabelState() { Text = "In Window" });
            //window.State.UI.Add(label);

            var label = new TvLabel(skin, new Viewport(new TvPoint(0, 0), 9, 1, 0), new LabelState() { Text = "In Window" });
            var dialog = _dialogManager.CreateDialog(vpf.FullViewport().CreateCentered(20, 10),
                d =>
                {
                    d.State.UI.Add(label);
                });


            var sleft = TvStatexControl.Wrap<TvList, ListState, FileList>(left, opt =>
            {
                opt.UseStore("left");
                opt.UseStatexTransformation((fl, cs) =>
                {
                    cs.Clear();
                    cs.AddRange(fl.Items);
                });
            });
            var sright = TvStatexControl.Wrap<TvList, ListState, FileList>(right, opt =>
            {
                opt.UseStore("right");
                opt.UseStatexTransformation((fl, cs) =>
                {
                    cs.Clear();
                    cs.AddRange(fl.Items);
                });
            });

            grid.Use(0, 0).Add(left);
            grid.Use(0, 1).Add(right);
            tui.UI.Add(grid);
            var bottom = new TvStackPanel(tui.UI, "BottomContainer");
            bottom.AsComponent().AddViewport(vpf.BottomViewport(2));
            bottom.Children.Add(textbox);
            tui.UI.Add(bottom);
            tui.UI.Add(menu);

            //tui.UI.Add(window);
            tui.UI.Add(dialog);

            _storeSelector.GetStore<FileList>("left").Dispatch(new TvAction<string>("FETCH_DIR", "C:\\"));
            _storeSelector.GetStore<FileList>("right").Dispatch(new TvAction<string>("FETCH_DIR", "D:\\"));
            return Task.CompletedTask;
        }
    }
}
