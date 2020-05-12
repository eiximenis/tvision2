using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Core.Components.Behaviors
{


    public class FactoryBehaviorMetadata<TB, T> : IFactoryBehaviorMetadata<TB, T>
        where TB : ITvBehavior<T>
    {
        private Action<TB> _afterCreate;
        public ITvBehavior<T> Behavior { get; private set; }

        ITvBehavior IBehaviorMetadata.Behavior { get => Behavior; }

        public bool Created => Behavior != null;

        public BehaviorSchedule Schedule { get; private set; }

        void IBehaviorMetadata.CreateBehavior(IServiceProvider sp)
        {
            if (!Created)
            {
                var type = typeof(TB);
                var ctors = type.GetConstructors();
                if (ctors.Length != 1)
                {
                    throw new InvalidOperationException($"Behavior {typeof(TB).Name} must have ONE and only one <ctor>.");
                }
                var ctor = ctors.Single();
                var parameters = new ArrayList();
                foreach (var param in ctor.GetParameters())
                {
                    parameters.Add(sp.GetService(param.ParameterType));
                }
                var behavior = ctor.Invoke(parameters.ToArray()) as ITvBehavior<T>;
                _afterCreate?.Invoke((TB)behavior);
                Behavior = behavior;
            }
        }

        public IBehaviorMetadata<T> UseScheduler(BehaviorSchedule schedule)
        {
            Schedule = schedule;
            return this;
        }

        public void OnCreate(Action<TB> afterCreate)
        {
            _afterCreate = afterCreate;
        }
    }
}
