﻿using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Styles;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts.Canvas;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;

namespace Tvision2.Dialogs
{
    public class DialogState : IDirtyObject
    {

        private readonly string _prefixNames;
        private readonly ISkin _skin;

        private TvDialog _myDialog;
        private TvStackPanel _mainPanel;
        private TvGrid _bottomGrid;
        private TvCanvas _mainCanvas;

        public IComponentTree UI => _mainCanvas.Children;

        public DialogState(ISkin skin, string prefix)
        {
            _skin = skin;
            _prefixNames = prefix;
        }

        internal void Init(TvDialog dialog, IComponentTree owner)
        {
            _myDialog = dialog;
            var viewport = _myDialog.AsComponent().Viewport.Layer(ViewportLayer.Top);
            _mainPanel = new TvStackPanel(owner, $"{_prefixNames}_MainPanel");
            _mainPanel.Layout.Add("1", "*");
            _mainPanel.AsComponent().AddViewport(new Viewport(viewport.Position + new TvPoint(1, 1), viewport.Columns - 2, viewport.Rows - 2, viewport.ZIndex));
            _bottomGrid = new TvGrid(owner, new GridState(1, 2), $"{_prefixNames}_BottomGrid");
            _bottomGrid.Use(0, 0)
                .Add(new TvButton(_skin, Viewport.NullViewport, new ButtonState() { Text = "Ok" }));
            _bottomGrid.Use(0, 1).Add(new TvButton(_skin, Viewport.NullViewport, new ButtonState() { Text = "Cancel" }));
            _mainCanvas = new TvCanvas(owner, $"{_prefixNames}_BodyCanvas");
            _mainCanvas.AsComponent().AddViewport(Viewport.NullViewport);

            _mainPanel.At(0).Add(_mainCanvas);
            _mainPanel.At(1).Add(_bottomGrid);

            IsDirty = true;
        }

        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;
    }
}