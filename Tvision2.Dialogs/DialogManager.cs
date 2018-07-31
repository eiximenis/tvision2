using System;
using Tvision2.Controls.Styles;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Dialogs
{
    class DialogManager : IDialogManager
    {
        private readonly ISkinManager _skinManager;
        private readonly IComponentTree _ui;
        public DialogManager(ISkinManager skinManager, ITuiEngine engine)
        {
            _skinManager = skinManager;
            _ui = engine.UI;
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
    }
}
