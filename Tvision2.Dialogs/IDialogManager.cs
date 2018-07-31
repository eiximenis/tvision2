using System;
using Tvision2.Core.Render;

namespace Tvision2.Dialogs
{
    public interface IDialogManager
    {
        TvDialog CreateDialog(IViewport viewport, Action<TvDialog> dialogSetup, string name = null);
    }
}
