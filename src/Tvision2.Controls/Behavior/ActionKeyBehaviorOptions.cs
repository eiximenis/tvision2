using System;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Behavior
{
    public class ActionKeyBehaviorOptions<TState>
    {
        public Func<TvConsoleKeyboardEvent, bool> Predicate { get; set; }

        public Func<TvConsoleKeyboardEvent, BehaviorContext<TState>, bool> Action { get; set; }
    }
}