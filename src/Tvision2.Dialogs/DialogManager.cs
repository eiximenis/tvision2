using System;
using System.Collections.Generic;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Dialogs
{
    class DialogManager : IDialogManager
    {
        private readonly ISkinManager _skinManager;
        private readonly IComponentTree _ui;

        private readonly Stack<TvDialog> _dialogs;

        public TvDialog? CurrentDialog
        {
            get => _dialogs.TryPeek(out var dialog) ? dialog : null;
        }

        public DialogManager(ISkinManager skinManager, ITuiEngine engine)
        {
            _dialogs = new Stack<TvDialog>();
            _skinManager = skinManager;
            _ui = engine.UI;
        }

        public TvDialog CreateDialog(IViewport viewport, Action<TvDialog> dialogSetup, string name = null)
        {
            var dialogViewport = viewport.Layer(Layer.Top, -1);
            var dialogName = name ?? $"TvDialog_{Guid.NewGuid()}";
            var dialog = new TvDialog(_skinManager.CurrentSkin, dialogViewport, _ui, dialogName);
            dialogSetup.Invoke(dialog); 
            
            foreach (var cmp in dialog.State.UI)
            {
                cmp.UpdateViewport(cmp.Viewport.Layer(Layer.Top));
            }
            
            return dialog;
        }

        public void CloseDialog()
        {
            if (CurrentDialog != null)
            {
                CurrentDialog.Close();
                _dialogs.Pop();
            }
        }

        public void ShowDialog(TvDialog dialog)
        {
            _ui.Add(dialog);
            _dialogs.Push(dialog);
        }
    }
}
