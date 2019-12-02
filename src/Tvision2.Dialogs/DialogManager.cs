using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls;
using Tvision2.Controls.Styles;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Dialogs
{
    class DialogManager : IDialogManager
    {
        private readonly ISkinManager _skinManager;
        private readonly IComponentTree _ui;
        private readonly List<TvControlMetadata> _outsideDialogControls;
        private readonly IControlsTree _rootControls;

        public TvDialog DialogShown { get; private set; }

        public DialogManager(ISkinManager skinManager, ITuiEngine engine, IControlsTree rootControls)
        { 
            _skinManager = skinManager;
            _ui = engine.UI;
            _outsideDialogControls = new List<TvControlMetadata>();
            _rootControls = rootControls;
        }

        public TvDialog CreateDialog(IViewport viewport, Action<TvDialog> dialogSetup, string name = null)
        {
            var dialogViewport = viewport.Layer(ViewportLayer.Top, -1);
            var dialogName = name ?? $"TvDialog_{Guid.NewGuid()}";
            var dialog = new TvDialog(_skinManager.CurrentSkin, dialogViewport, _ui, dialogName);
            dialogSetup.Invoke(dialog); 
            foreach (var cmp in dialog.State.UI.Components)
            {
                cmp.UpdateViewport(cmp.Viewport.Layer(ViewportLayer.Top));
            }
            return dialog;
        }

        public void CloseDialog()
        {
            if (DialogShown != null)
            {
                DialogShown.Close();
                foreach (var metadata in _outsideDialogControls)
                {
                    metadata.RestoreFocusability();
                }
                _outsideDialogControls.Clear();
            }
        }

        public void ShowDialog(TvDialog dialog)
        {
            _ui.Add(dialog, _ =>
            {
                var insideControls = dialog.State.UI.OwnedControls(_rootControls).Union(dialog.State.Buttons.Select(b => b.Metadata));

                foreach (var controlMetadata in _rootControls.ControlsMetadata)
                {
                    if (!insideControls.Contains(controlMetadata))
                    {
                        controlMetadata.DisableFocusability();
                        _outsideDialogControls.Add(controlMetadata);
                    }
                }
            });
            DialogShown = dialog;
        }
    }
}
