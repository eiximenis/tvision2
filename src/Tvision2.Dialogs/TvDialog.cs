using System;
using System.Collections.Generic;
using Tvision2.Controls;
using Tvision2.Controls.Drawers;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts;
using Tvision2.Layouts.Canvas;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;
using Tvision2.Styles;
using Tvision2.Styles.Drawers;
using Tvision2.Styles.Extensions;

namespace Tvision2.Dialogs
{
    public class TvDialog : TvControl<DialogState>
    {
        private readonly TvStackPanel _mainPanel;
        private readonly DialogButtons _buttons;
        private readonly TvGrid _bottomGrid;
        private readonly TvCanvas _mainCanvas;
        private readonly IComponentTree _owner;
        private IControlsTree? _controlsTree = null;

        public IComponentsCollection UI { get => _mainCanvas; }

        internal TvDialog(ISkin skin, IViewport viewport, IComponentTree owner, string name)
            : base(new TvControlCreationParameters<DialogState>(
                skin, viewport.Layer(Layer.Top, -1),
                new DialogState(skin, name ?? $"TvDialog_{Guid.NewGuid()}"), mustCreateViewport: false))
        {
            _owner = owner;
            _mainPanel = new TvStackPanel($"{name}_MainPanel");
            _buttons = new DialogButtons(skin);
            _buttons.AddOkButton();
            _buttons.AddCancelButton();
            var topViewport = viewport.Layer(Layer.Top);

            _mainPanel = new TvStackPanel($"{name}_MainPanel");
            _mainPanel.Layout.Add("*", "1");
            _mainPanel.AsComponent().AddViewport(new Viewport(viewport.Position + TvPoint.FromXY(1, 1), TvBounds.FromRowsAndCols(viewport.Bounds.Rows - 2, viewport.Bounds.Cols - 2), viewport.ZIndex));
            _bottomGrid = new TvGrid(new GridState(1, 2), null, $"{name}_BottomGrid");
            _bottomGrid.AtRowCol(0, 0).WithAlignment(ChildAlignment.Fill).Add(_buttons.OkButton.AsComponent());
            _bottomGrid.AtRowCol(0, 1).WithAlignment(ChildAlignment.Fill).Add(_buttons.CancelButton.AsComponent());
            _mainCanvas = new TvCanvas(_owner, $"{name}_BodyCanvas");
            _mainCanvas.AsComponent().AddViewport(Viewport.NullViewport);
            _mainPanel.At(0).Add(_mainCanvas);
            _mainPanel.At(1).Add(_bottomGrid);
            _owner.AddAsChild(_mainPanel.AsComponent(), this.AsComponent());
            _owner.OnAddingIdleCycle.AddOnce(_ => CaptureFocus());
            State.Init(this, _owner);
        }

        protected override void AddCustomElements(TvComponent<DialogState> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle, BorderValue.FromHorizontalAndVertical(BorderType.Double, BorderType.Double)));
        }

        protected override void OnDraw(RenderContext<DialogState> context)
        {
            var pairIdx = CurrentStyle.Standard;
            for (var row = 1; row < context.Viewport.Bounds.Rows - 2; row++)
            {
                context.DrawChars(' ', context.Viewport.Bounds.Cols - 2, TvPoint.FromXY(1, row), pairIdx);
            }
            context.Fill(pairIdx);
        }

        public void Add(ITvControl controlToAdd)
        {
            _mainCanvas.Add(controlToAdd);
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.FocusedWhen().Never();
        }

        protected override void OnControlMounted(ITuiEngine owner)
        {
            _controlsTree = owner.GetControlsTree();
            return;
            
        }

        private void CaptureFocus()
        {
            Metadata.CaptureFocus();
            _controlsTree?.MoveFocusToNext();
        }

        internal void Close()
        {
            _owner.Remove(this);
            State.Destroy();

            // TODO: Review this -> It is easy to solve
            /*
            State.UI.Engine.UI.Remove(State.MainPanel.AsComponent());
            State.UI.Engine.UI.Remove(AsComponent());

            */
        }
    }
}
