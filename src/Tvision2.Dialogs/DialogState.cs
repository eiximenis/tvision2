﻿using System;
using System.Collections.Generic;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts;
using Tvision2.Layouts.Canvas;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;
using Tvision2.Styles;

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
        private DialogButtons _buttons;
        private IComponentTree _owner;

        public IComponentsCollection UI => _mainCanvas;

        public IEnumerable<TvButton> Buttons => _buttons;

        internal TvStackPanel MainPanel => _mainPanel;


        public DialogState(ISkin skin, string prefix)
        {
            _skin = skin;
            _prefixNames = prefix;
            _buttons = new DialogButtons(skin);
        }

        internal void Init(TvDialog dialog, IComponentTree owner)
        {
            _myDialog = dialog;
            _owner = owner;
            _buttons.AddOkButton();
            _buttons.AddCancelButton();
            var viewport = _myDialog.AsComponent().Viewport.Layer(Layer.Top);
            
            _mainPanel = new TvStackPanel($"{_prefixNames}_MainPanel");
            _mainPanel.Layout.Add("*", "1");
            _mainPanel.AsComponent().AddViewport(new Viewport(viewport.Position + TvPoint.FromXY(1, 1), TvBounds.FromRowsAndCols(viewport.Bounds.Rows -2, viewport.Bounds.Cols - 2), viewport.ZIndex));
            _bottomGrid = new TvGrid (new GridState(1, 2), null, $"{_prefixNames}_BottomGrid");
            _bottomGrid.AtRowCol(0, 0).WithAlignment(ChildAlignment.Fill).Add(_buttons.OkButton.AsComponent());
            _bottomGrid.AtRowCol(0, 1).WithAlignment(ChildAlignment.Fill).Add(_buttons.CancelButton.AsComponent());
            _mainCanvas = new TvCanvas(_owner, $"{_prefixNames}_BodyCanvas");
            _mainCanvas.AsComponent().AddViewport(Viewport.NullViewport);
            _mainPanel.At(0).Add(_mainCanvas);
            _mainPanel.At(1).Add(_bottomGrid);
            _owner.AddAsChild(_mainPanel.AsComponent(), dialog.AsComponent());
            IsDirty = true;
        }

        internal void Destroy()
        {
            _owner.Remove(_myDialog);
            _owner = null;
        }

        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;
    }
}