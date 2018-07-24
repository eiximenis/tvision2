using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Checkbox;
using Tvision2.Controls.Label;
using Tvision2.Controls.Styles;
using Tvision2.Controls.Textbox;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Sample
{
    public class Startup : ITvisionAppStartup
    {
        private readonly ISkinManager _skinManager;
        public Startup(ISkinManager skinManager)
        {
            _skinManager = skinManager;
        }
        Task ITvisionAppStartup.Startup(ITuiEngine tui)
        {
            var skin = _skinManager.CurrentSkin;

            var lbl = new TvLabel(skin, new Viewport(new TvPoint(0, 0), 2, 1, 0), new LabelState()
            {
                Text = "label",
            });
            lbl.Viewport.MoveTo(new TvPoint(4, 3));
            tui.UI.Add(lbl);

            var check = new TvCheckbox(skin, new Viewport(new TvPoint(0, 0), 10, 1, 0), new CheckboxState());
            check.Viewport.MoveTo(new TvPoint(8, 7));
            tui.UI.Add(check);

            var button = new TvButton(skin, new Viewport(new TvPoint(0, 0), 4, 1, 0), new ButtonState()
            {
                Text = "btn"
            });

            button.Viewport.MoveTo(new TvPoint(4, 6));
            button.Viewport.Grow(4, 0);

            button.OnClick = new DelegateCommand<ButtonState>(async s =>
            {
                lbl.State.Text = "Fuck yeah!";
                check.State.Checked = TvCheckboxState.Checked;
            });

            button.AsComponent().AddViewport(new Viewport(new TvPoint(7, 8), 10));

            tui.UI.Add(button);

            var textbox = new TvTextbox(skin, new Viewport(new TvPoint(12, 9), 8, 1, 0), new TextboxState());
            tui.UI.Add(textbox);
            //await Task.Delay(6000);
            //button.State.Text = "---Uuuups";

            return Task.CompletedTask;
        }
    }
}
