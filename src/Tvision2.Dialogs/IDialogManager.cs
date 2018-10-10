using System;
using Tvision2.Core.Render;

namespace Tvision2.Dialogs
{
    public interface IDialogManager
    {
        void ShowDialog(TvDialog dialog);
        TvDialog CreateDialog(IViewport viewport, Action<TvDialog> dialogSetup, string name = null);
        TvDialog DialogShown { get; }
        void CloseDialog();
    }
}
