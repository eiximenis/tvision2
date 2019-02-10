using System.Collections;
using System.Collections.Generic;
using Tvision2.Controls.Button;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

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
            CancelButton = new TvButton(TvButton.CreationParametersBuilder(s =>
            {
                s.Text = "Cancel";
            }).UseSkin(_skin).UseViewport(Viewport.NullViewport));
        }

        public void AddOkButton()
        {
            OkButton = new TvButton(TvButton.CreationParametersBuilder(s =>
            {
                s.Text = "Ok";
            }).UseSkin(_skin).UseViewport(Viewport.NullViewport));
        }

        public IEnumerator<TvButton> GetEnumerator()
        {
            yield return OkButton;
            yield return CancelButton;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
