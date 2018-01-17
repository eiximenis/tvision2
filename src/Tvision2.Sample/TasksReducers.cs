using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Stores;

namespace Tvision2.Sample
{
    static class TasksReducers
    {
        public static TasksList AddTask(TasksList state, TvAction action)
        {
            if (action.Name == "ADD_TASK")
            {
                var num = state.Count;
                var newState = new TasksList();
                newState.AddRange(state);
                newState.Add(new TaskData() { Message = $"I am task #{num + 1}" });
                return newState;
            }

            return state;
        }
    }
}
