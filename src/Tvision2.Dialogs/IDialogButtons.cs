using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Button;
using Tvision2.Core.Render;

namespace Tvision2.Dialogs
{
    public interface IDialogButtons : IEnumerable<TvButton>
    {
        void AddOkButton();
        void AddCancelButton();

        TvButton OkButton { get; }
        TvButton CancelButton { get; }
    }
}
