using System;
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
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var tui = new TuiEngineBuilder()
                .AddTvControls()
                .AddStateManager(mgr =>
                {
                    mgr.AddStore<TasksStore, TasksList>("Tasks", new TasksStore())
                        .AddReducer(TasksReducers.AddTask);
                })
                .AddSkinSupport(sb =>
                {
                    sb.AddMcStyles();
                })
                .Build();

            var skin = tui.SkinManager().CurrentSkin;

            var lbl = new TvLabel(skin, new BoxModel(new TvPoint(0,0),2,1),  new LabelState()
            {
                Text = "label",
            });
            lbl.Style.Position = new Core.Render.TvPoint(3, 4);
            tui.UI.Add(lbl);

            var check = new TvCheckbox(skin, new BoxModel(new TvPoint(0, 0), 10, 1), new CheckboxState());
            check.Style.Position = new TvPoint(7, 8);
            tui.UI.Add(check);

            var button = new TvButton(skin, new BoxModel(new TvPoint(0, 0), 4, 1), new ButtonState()
            {
                Text = "btn"
            });

            button.Style.Position = new Core.Render.TvPoint(0, 0);
            button.Style.Columns = 4;

            button.OnClick = new DelegateCommand<ButtonState>(async s =>
            {
                lbl.State.Text = "Fuck yeah!";
                check.State.Checked = TvCheckboxState.Checked;
            });

            tui.UI.Add(button);

            var textbox = new TvTextbox(skin, new BoxModel(new TvPoint(9, 12), 8, 1), new TextboxState());
            tui.UI.Add(textbox);

            var t = tui.Start();
            await Task.Delay(6000);
            button.State.Text = "---Uuuups";
            await t;

            return 0;
        }

        private static AppliedStyle StyleSheet()
        {
            throw new NotImplementedException();
        }
    }
}
