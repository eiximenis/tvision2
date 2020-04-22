using System.Collections.Generic;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
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
        private DialogButtons _buttons;

        public IComponentsCollection UI => _mainCanvas.Children;

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
            _buttons.AddOkButton();
            _buttons.AddCancelButton();
            var viewport = _myDialog.AsComponent().Viewport.Layer(Layer.Top);
            _mainPanel = new TvStackPanel($"{_prefixNames}_MainPanel");
            _mainPanel.Layout.Add("1", "*");
            _mainPanel.AsComponent().AddViewport(new Viewport(viewport.Position + TvPoint.FromXY(1, 1), TvBounds.FromRowsAndCols(viewport.Bounds.Rows -2, viewport.Bounds.Cols - 2), viewport.ZIndex));
            _bottomGrid = new TvGrid (new GridState(1, 2), $"{_prefixNames}_BottomGrid");
            _bottomGrid.At(0, 0).Add(_buttons.OkButton);
            _bottomGrid.At(0, 1).Add(_buttons.CancelButton);
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