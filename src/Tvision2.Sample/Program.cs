using System;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Checkbox;
using Tvision2.Controls.Label;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Core.Styles;

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
                .AddSkinSupport(opt =>
                {
                    opt.AddSkin("Ugly", b =>
                    {
                        b.AddStyleSheet("Test", ss =>
                        {
                            ss.AddClass("focused", s =>
                            {
                                s.WithBackgroundColor(ConsoleColor.Yellow);
                                s.WithForegroundColor(ConsoleColor.Black);
                            });
                        });
                    });

                })
                .Build();

            var cmp = new TestComponent(new AppliedStyle(ClippingMode.Clip), "test");
            cmp.AddBehavior(new TestBehavior(tui.StoreSelector()));

            tui.UI.Add(cmp);

            var lbl = new TvLabel(new LabelState("Label1")
            {
                Text = "label",
            });

            lbl.Data.Style.Position = new Core.Render.TvPoint(3, 4);

            var check = new TvCheckbox(new CheckboxState());
            check.Data.Style.Position = new TvPoint(7, 8);
            tui.UI.Add(check);


            var button = new TvButton(new ButtonState()
            {
                Text = "btn"
            });

            button.Data.Style.Position = new Core.Render.TvPoint(0, 0);
            button.Data.Style.Columns = 4;

            button.OnClick = new DelegateCommand<ButtonState>(async s =>
            {
                lbl.State.Text = "Fuck yeah!";
                check.State.Checked = TvCheckboxState.Checked;
            });

            tui.UI.Add(lbl);
            tui.UI.Add(button, 0);


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
