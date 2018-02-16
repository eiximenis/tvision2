using System;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Controls.Label;
using Tvision2.Core;
using Tvision2.Core.Engine;

namespace Tvision2.Sample
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var tui = new TuiEngineBuilder()
                .AddStateManager(mgr =>
                {
                    mgr.AddStore<TasksStore, TasksList>("Tasks", new TasksStore())
                        .AddReducer(TasksReducers.AddTask);
                })
                .UseConsoleDriver(ConsoleDriverType.NetDriver)
                .Build();

            var cmp = new TestComponent(new Core.Styles.StyleSheet(), "test");
            cmp.AddBehavior(new TestBehavior(tui.StoreSelector()));

            tui.UI.Add(cmp);

            var lbl = new TvLabel(new LabelState()
            {
                Text = "label",
                Name = "Label1",
            });

            lbl.Data.Style.Position = new Core.Render.TvPoint(3, 4);

            var button = new TvButton(new ButtonState()
            {
                Text = "btn"
            });

            button.OnClick = new DelegateCommand<ButtonState>(async s =>
            {
                lbl.State.Text = "Fuck yeah!";
            });

            tui.UI.Add(lbl);
            tui.UI.Add(button, -1);

            var t = tui.Start();
            await Task.Delay(6000);
            button.State.Text = "---Uuuups";
            await t;

            return 0;
        }
    }
}
