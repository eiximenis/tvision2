using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components.Behaviors
{
    public abstract class OptionsBehavior<TOptions,T> : ITvBehavior<T>
        where TOptions : new()
    {

        private readonly TOptions _options;

        protected TOptions Options => _options;

        public OptionsBehavior(Action<TOptions> optionsAction)
        {
            _options = new TOptions();
            optionsAction?.Invoke(_options);
        }

        bool ITvBehavior.Update(BehaviorContext updateContext) => Update((BehaviorContext<T>)updateContext);

        public abstract bool Update(BehaviorContext<T> updateContext);
    }
}
