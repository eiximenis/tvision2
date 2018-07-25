using Tvision2.Controls;
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

        private ListComponentTree _children;
        private TvDialog _myDialog;

        private readonly TvStackPanel _mainPanel;
        private readonly TvGrid _bottomGrid;
        private readonly ISkin _skin;
        private readonly TvCanvas _mainCanvas;

        public IComponentTree UI => _mainCanvas.Children;

        public DialogState(ISkin skin, IComponentTree owner, IViewport viewport)
        {
            _skin = skin;
            _mainPanel = new TvStackPanel(owner);
            _mainPanel.AsComponent().AddViewport(new Viewport(viewport.Position + new TvPoint(1, 1), viewport.Columns - 2, viewport.Rows - 2, viewport.ZIndex));
            _bottomGrid = new TvGrid(owner, new GridState(1, 2));
            _bottomGrid.Use(0, 0).Add(new TvButton(_skin, Viewport.NullViewport, new ButtonState() { Text = "Ok" }));
            _bottomGrid.Use(0, 1).Add(new TvButton(_skin, Viewport.NullViewport, new ButtonState() { Text = "Cancel" }));
            _mainCanvas = new TvCanvas(owner);
            _mainPanel.Children.Add(_mainCanvas);
            _mainPanel.Children.Add(_bottomGrid);
        }

        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;


        internal void SetOwnerDialog(TvDialog ownerDialog)
        {
            _myDialog = ownerDialog;
            foreach (var existingChild in _children.Components)
            {
                existingChild.UpdateViewport(existingChild.Viewport.InnerViewport(_myDialog.Viewport, new TvPoint(1, 1)));
            }
            IsDirty = true;
        }
    }
}