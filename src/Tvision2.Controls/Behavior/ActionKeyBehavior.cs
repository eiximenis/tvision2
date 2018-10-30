using System.Linq;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Controls.Behavior
{
    public class ActionKeyBehavior<TState> : ITvBehavior<TState>
    {

        private readonly ActionKeyBehaviorOptions<TState> _options;
        public ActionKeyBehavior(ActionKeyBehaviorOptions<TState> options) => _options = options;


        public bool Update(BehaviorContext<TState> updateContext)
        {

            bool updated = false;
            if (updateContext.Events.HasKeyboardEvents)
            {
                foreach (var evt in updateContext.Events.KeyboardEvents.Where(e => _options.Predicate(e)))
                {
                    evt.Handle();
                    updated = updated || _options.Action(evt, updateContext);
                }
            }

            return updated;
        }

        bool ITvBehavior.Update(BehaviorContext updateContext) => Update(updateContext as BehaviorContext<TState>);
    }
}
