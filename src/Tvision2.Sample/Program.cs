﻿using System;
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
            var tui = new TuiEngineBuilder().Build();

            tui.StateManager.
                AddStore<TasksStore, TasksList>("Tasks", new TasksStore())
                .AddReducer(TasksReducers.AddTask);

            var lbl = new TvLabel(new LabelState()
            {
                Text = "label",
                Name = "Label1"
            });

            var button = new TvButton(new ButtonState()
            {
                Text="btn"
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
