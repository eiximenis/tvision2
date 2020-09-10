using System.Collections;
using System.Collections.Generic;
using Tvision2.Controls.Button;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Dialogs
{
    public class DialogButtons : IDialogButtons
    {
        public TvButton OkButton { get; private set; }
        public TvButton CancelButton { get; private set; }
        private readonly ISkin _skin;

        public DialogButtons(ISkin dialogSkin)
        {
            _skin = dialogSkin;
        }

        public void AddCancelButton()
        {
            var cparams = TvButton.UseParams().WithState(ButtonState.FromText("Cancel"))
                .Configure(c => c.UseSkin(_skin).UseViewport(Viewport.NullViewport))
                .Build();
            CancelButton = new TvButton(cparams);
        }

        public void AddOkButton()
        {
            var okparams = TvButton.UseParams().WithState(ButtonState.FromText("Ok"))
                .Configure(c => c.UseSkin(_skin).UseViewport(Viewport.NullViewport))
                .Build();
            OkButton = new TvButton(okparams);
        }

        public IEnumerator<TvButton> GetEnumerator()
        {
            yield return OkButton;
            yield return CancelButton;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
