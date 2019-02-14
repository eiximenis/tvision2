using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Dialogs.Extensions
{
    public static class TuiEngineExtensions
    {
        public static IDialogManager DialogManager(this ITuiEngine engine)
        {
            return engine.ServiceProvider.GetService(typeof(IDialogManager)) as IDialogManager;
        }
    }
}
