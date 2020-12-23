using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.ActivityIndicator;
using Tvision2.Controls.Label;
using Tvision2.Controls.List;
using Tvision2.Controls.Menu;
using Tvision2.Controls.Textbox;
using Tvision2.Core;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Dialogs;
using Tvision2.Layouts;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;
using Tvision2.MidnightCommander.Stores;
using Tvision2.Statex;
using Tvision2.Statex.Controls;
using Tvision2.Styles;

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

            var mainStackPanel = new TvStackPanel("mainStackPanel");
            mainStackPanel.AsComponent().AddViewport(dvpf.Create(vpf => vpf.FullViewport()));
            tui.UI.Add(mainStackPanel);
            mainStackPanel.Layout.Add("1", "*", "3");
            var listFilesGrid = new TvGrid(new GridState(1, 2), null, "FilesGrid");
            mainStackPanel.At(1).Add(listFilesGrid);

            var textboxParams = TvTextbox.UseParams().WithDefaultState().Configure(c => c.UseViewport(null)).Build();
            var textbox = new TvTextbox(textboxParams);

            var activityParams = TvActivityIndicator.UseParams().WithDefaultState().Configure(c => c.UseViewport(new Viewport(TvPoint.FromXY(0, 22), TvBounds.FromRowsAndCols(1, 1), Layer.Standard))).Build();
            var actind = new TvActivityIndicator(activityParams);
            tui.UI.Add(actind);


            var leftParams = TvList.UseParams<FileItem>()
                .WithState(() => ListState<FileItem>
                        .From(Enumerable.Empty<FileItem>())
                        .AddFixedColumn(fi => fi.IsDirectory ? "*" : " ", width: 2)
                        .AddColumn(fi => fi.Name)
                        .Build())
                .Configure(c => c.UseViewport(new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, 10), Layer.Standard)))
                .Build();


            var left = new TvList<FileItem>(leftParams);

            left.StyleProvider
                .Use(Core.Colors.TvColor.Red)
                .When(f => (f.FileAttributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                .AppliesToColumn(1);

            var right = new TvList<FileItem>(new TvControlCreationParameters<ListState<FileItem>>(skin, new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, 10), Layer.Standard),
                new ListState<FileItem>(Enumerable.Empty<FileItem>(), new TvListColumnSpec<FileItem>() { Transformer = f => f.Name }), mustCreateViewport: false));

            right.StyleProvider
                .Use(Core.Colors.TvColor.Red, Core.Colors.TvColor.Blue)
                .When(f => (f.FileAttributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                .AppliesToColumn(1);


            var menuParams = TvMenuBar.UseParams()
                .Options(o => o.ItemsSpacedBy(4).UseHotKey(System.ConsoleKey.F9))
                .WithState(new MenuState(new[] { "_Left", "_Edit", "_Command", "_Options", "_Help", "_Right" })).Build();

            var menu = new TvMenuBar(menuParams);
            mainStackPanel.At(0).Add(menu);

            menu.State["Left"].AddChild("_Exit");
            menu.State["Edit"].AddChild("Delete");
            menu.State["Edit"].AddChild("Move");


            //var window = new TvWindow(skin, vpf.FullViewport().CreateCentered(20, 10), new WindowState(tui.UI));
            //var label = new TvLabel(skin, new Viewport(new TvPoint.FromXY(0, 0), 9, 1, 0), new LabelState() { Text = "In Window" });
            //window.State.UI.Add(label);

            var labelParams = TvLabel.UseParams().WithState(LabelState.FromText("In Window"))
                .Configure(c => c.UseViewport(new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, 9), Layer.Standard)))
                .Build();

            var label = new TvLabel(labelParams);

            var dialog = _dialogManager.CreateDialog
                    (dvpf.Create(vpf => vpf.FullViewport().CreateCentered(20, 10)),
                d => { d.State.UI.Add(label); });

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
                opt.On(c => c.OnItemClicked)
                    .Dispatch((s, args) => new TvAction<FileItem>("FETCH_INFO", args));

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
                opt.On(c => c.OnItemClicked)
                    .Dispatch((s, args) => new TvAction<FileItem>("FETCH_INFO", args));
            });

            listFilesGrid.At(0, 0).Add(left.AsComponent());
            listFilesGrid.At(0, 1).Add(right.AsComponent());
            var bottom = new TvStackPanel("BottomContainer");
            bottom.Layout.Add(new LayoutSize());
            bottom.At(0).Add(textbox);

            mainStackPanel.At(2).Add(bottom);

            //tui.UI.Add(window);

            //_dialogManager.ShowDialog(dialog);
            _storeSelector.GetStore<FileList>("left").Dispatch(new TvAction<string>("FETCH_DIR", "C:\\"));
            _storeSelector.GetStore<FileList>("right").Dispatch(new TvAction<string>("FETCH_DIR", "D:\\"));




            return Task.CompletedTask;
        }
    }
}