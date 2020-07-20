using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tvision2.BlazorDemo.Model;
using Tvision2.Controls;
using Tvision2.Controls.ActivityIndicator;
using Tvision2.Controls.List;
using Tvision2.Controls.Menu;
using Tvision2.Controls.Styles;
using Tvision2.Controls.Textbox;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;

namespace Tvision2.BlazorDemo
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
            var dvpf = _layoutManager.DynamicViewportFactory;

            var mainStackPanel = new TvStackPanel("mainStackPanel");
            mainStackPanel.AsComponent().AddViewport(dvpf.Create(vpf => vpf.FullViewport()));
            tui.UI.Add(mainStackPanel);
            mainStackPanel.Layout.Add("1", "*", "3");
            var listFilesGrid = new TvGrid(new GridState(1, 2));
            mainStackPanel.At(1).Add(listFilesGrid);
            var textbox = new TvTextbox(TvTextbox.CreationParametersBuilder().UseSkin(skin).UseViewport(null));
            var actind = new TvActivityIndicator(TvActivityIndicator.CreationParametersBuilder().UseSkin(skin).UseViewport(new Viewport(TvPoint.FromXY(0, 22), TvBounds.FromRowsAndCols(1, 1), Layer.Standard)));
            tui.UI.Add(actind);
            var left = new TvList<FileItem>(TvList.CreationParametersBuilder(
                () => ListState<FileItem>
                    .From(Enumerable.Empty<FileItem>())
                    .AddFixedColumn(fi => fi.IsDirectory ? "*" : " ", width: 2)
                    .AddColumn(fi => fi.Name)
                    .Build())
                .UseSkin(skin).UseViewport(new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, 10), Layer.Standard)));

            /*
            left.StyleProvider
                .Use(Core.Colors.TvColor.Red)
                .When(f => (f.FileAttributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                .AppliesToColumn(1);
            */

            var right = new TvList<FileItem>(new TvControlCreationParameters<ListState<FileItem>>(skin, new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, 10), Layer.Standard),
                new ListState<FileItem>(Enumerable.Empty<FileItem>(), new TvListColumnSpec<FileItem>() { Transformer = f => f.Name })));

            /*
            right.StyleProvider
                .Use(Core.Colors.TvColor.Red, Core.Colors.TvColor.Blue)
                .When(f => (f.FileAttributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                .AppliesToColumn(1);
            */

            var menu = new TvMenuBar(TvMenuBar.CreationParametersBuilder(new[] { "_Left", "_Edit", "_Command", "_Options", "_Help", "_Right" })
                .UseSkin(skin), opt =>
                {
                    opt.ItemsSpacedBy(4);
                    opt.UseHotKey(System.ConsoleKey.F9);
                });
            mainStackPanel.At(0).Add(menu);

            menu.State["Left"].AddChild("_Exit");
            menu.State["Edit"].AddChild("Delete");
            menu.State["Edit"].AddChild("Move");


            listFilesGrid.At(0, 0).Add(left);
            listFilesGrid.At(0, 1).Add(right);
            var bottom = new TvStackPanel("BottomContainer");
            bottom.Layout.Add(new LayoutSize());
            bottom.At(0).Add(textbox);

            mainStackPanel.At(2).Add(bottom);

            return Task.CompletedTask;

        }
    }
}
