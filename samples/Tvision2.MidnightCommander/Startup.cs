using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.ActivityIndicator;
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

        public Startup(ISkinManager skinManager, ILayoutManager layoutManager, ITvStoreSelector storeSelector,
            IDialogManager dialogManager)
        {
            _skinManager = skinManager;
            _layoutManager = layoutManager;
            _storeSelector = storeSelector;
            _dialogManager = dialogManager;
        }

        Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {
            var skin = _skinManager.CurrentSkin;
            var dvpf = _layoutManager.DynamicViewportFactory;

            var mainStackPanel = new TvStackPanel(tui.UI, "mainStackPanel");
            mainStackPanel.AsComponent().AddViewport(dvpf.Create(vpf => vpf.FullViewport().Translate(new TvPoint(0, 1)).Grow(0, -3)));
            mainStackPanel.Layout.Add("*", "3");
            var listFilesGrid = new TvGrid(tui.UI, new GridState(1, 2));
            mainStackPanel.At(0).Add(listFilesGrid);

            tui.UI.Add(mainStackPanel);

            // TODO: Implement BorderedPanel
            // mainStackPanel.At(1).Add(new BorderedPanel());

            var textbox = new TvTextbox(TvTextbox.CreationParametersBuilder().UseSkin(skin).UseViewport(null));
            //var left = new TvList<FileItem>(skin, new Viewport(new TvPoint(0, 0), 10, 1, 0),
            //    new ListState<FileItem>(Enumerable.Empty<FileItem>(), f => new TvListItem() { Text = f.Name }));
            var actind = new TvActivityIndicator(TvActivityIndicator.CreationParametersBuilder().UseSkin(skin).UseViewport(new Viewport(new TvPoint(0, 22),new TvBounds(1, 1), 0)));
            tui.UI.Add(actind);


            var left = new TvList<FileItem>(TvList.CreationParametersBuilder(
                    () => ListState<FileItem>
                        .From(Enumerable.Empty<FileItem>())
                        .AddFixedColumn(fi => fi.IsDirectory ? "*" : " ", width: 2)
                        .AddColumn(fi => fi.Name)
                        .Build())
                    .UseSkin(skin).UseViewport(new Viewport(new TvPoint(0, 0), new TvBounds(1,10), 0)));

            left.StyleProvider
                .Use(Core.Colors.TvColor.Red)
                .When(f => (f.FileAttributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                .AppliesToColumn(1);

            var right = new TvList<FileItem>(new TvControlCreationParameters<ListState<FileItem>>(skin, new Viewport(new TvPoint(0, 0),new TvBounds(1, 10), 0),
                new ListState<FileItem>(Enumerable.Empty<FileItem>(), new TvListColumnSpec<FileItem>() { Transformer = f => f.Name })));

            right.StyleProvider
                .Use(Core.Colors.TvColor.Red, Core.Colors.TvColor.Blue)
                .When(f => (f.FileAttributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                .AppliesToColumn(1);

            var menu = new TvMenuBar(TvMenuBar.CreationParametersBuilder(new[] { "_Left", "_Edit", "_Command", "_Options", "_Help", "_Right" })
                .UseSkin(skin).UseViewport(dvpf.Create(vpf => vpf.FullViewport().TakeRows(1, 0))), opt =>
                 {
                     opt.ItemsSpacedBy(4);
                     opt.UseHotKey(System.ConsoleKey.F9);
                 });

            //var window = new TvWindow(skin, vpf.FullViewport().CreateCentered(20, 10), new WindowState(tui.UI));
            //var label = new TvLabel(skin, new Viewport(new TvPoint(0, 0), 9, 1, 0), new LabelState() { Text = "In Window" });
            //window.State.UI.Add(label);

            var label = new TvLabel(TvLabel.CreationParametersBuilder(s => s.Text = "In Window" )
                .UseSkin(skin).UseViewport(new Viewport(new TvPoint(0, 0), new TvBounds(1,9), 0)));
            /*
            var dialog = _dialogManager.CreateDialog(dvpf.Create(vpf => vpf.FullViewport().CreateCentered(20, 10)),
                d => { d.State.UI.Add(label); });
            */
            var sleft = TvStatexControl.Wrap<TvList<FileItem>, ListState<FileItem>, FileList>(left, opt =>
            {
                opt.UseDefaultStore("left");
                opt.UseStatexTransformation((fl, cs) =>
                {
                    cs.Clear();
                    cs.AddRange(fl.Items);
                });
                opt.On(c => c.OnItemClicked)
                    .Dispatch((s, args) => new TvAction<string>("FETCH_DIR", args.FullName))
                    .When(f => f.IsDirectory);

                /*
                opt.OnKeyEvent(e => e.AsConsoleKeyInfo().Key == System.ConsoleKey.Enter)
                    .Dispatch(s => new TvAction<FileItem>("FETCH_INFO", s[s.SelectedIndex]));
                */

                opt.OnKeyEvent(e => e.AsConsoleKeyInfo().Key == System.ConsoleKey.Backspace)
                    .Dispatch(s => new TvAction("FETCH_BACK"));

                opt.OnKeyEvent(e => e.AsConsoleKeyInfo().Key == System.ConsoleKey.F2)
                    .DispatchTo("GlobalStore", s => new TvAction<FileItem>("BEGIN_RENAME", s[s.SelectedIndex]));

            });
            var sright = TvStatexControl.Wrap<TvList<FileItem>, ListState<FileItem>, FileList>(right, opt =>
            {
                opt.UseDefaultStore("right");
                opt.UseStatexTransformation((fl, cs) =>
                {
                    cs.Clear();
                    cs.AddRange(fl.Items);
                });
                opt.On(c => c.OnItemClicked)
                .Dispatch((s, args) => new TvAction<string>("FETCH_DIR", args.FullName))
                .When(f => f.IsDirectory);
            });

            listFilesGrid.Use(0, 0).Add(left);
            listFilesGrid.Use(0, 1).Add(right);
            var bottom = new TvStackPanel(tui.UI, "BottomContainer");
            bottom.Layout.Add(new LayoutSize());
            bottom.AsComponent().AddViewport(dvpf.Create(vpf => vpf.BottomViewport(2)));
            bottom.At(0).Add(textbox);
            tui.UI.Add(bottom);
            tui.UI.Add(menu);

            //tui.UI.Add(window);

            //_dialogManager.ShowDialog(dialog);

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _storeSelector.GetStore<FileList>("left").Dispatch(new TvAction<string>("FETCH_DIR", "/"));
                _storeSelector.GetStore<FileList>("right").Dispatch(new TvAction<string>("FETCH_DIR", "/home/eiximenis"));
            }
            else
            {
                _storeSelector.GetStore<FileList>("left").Dispatch(new TvAction<string>("FETCH_DIR", "C:\\"));
                _storeSelector.GetStore<FileList>("right").Dispatch(new TvAction<string>("FETCH_DIR", "D:\\"));
            }


            return Task.CompletedTask;
        }
    }
}