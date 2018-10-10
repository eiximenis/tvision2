using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Button;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;
using Tvision2.Layouts.Grid;

namespace Tvision2.Dialogs
{
    public class DialogButtons : IDialogButtons
    {
        public TvButton OkButton { get; private set; }
        public TvButton CancelButton { get; private set; }
        private ISkin _skin;

        public DialogButtons(ISkin dialogSkin)
        {
            _skin = dialogSkin;
        }

        public void AddCancelButton()
        {
            CancelButton = new TvButton(_skin, Viewport.NullViewport, new ButtonState()
            {
                Text = "Cancel"
            });

        }

        public void AddOkButton()
        {
            OkButton = new TvButton(_skin, Viewport.NullViewport, new ButtonState()
            {
                Text = "Ok"
            });
        }

        public IEnumerator<TvButton> GetEnumerator()
        {
            yield return OkButton;
            yield return CancelButton;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
