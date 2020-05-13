using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static Tvision2.Core.Engine.ComponentTree;

namespace Tvision2.Core.Components.Behaviors
{


    public class FactoryBehaviorMetadata<TB, T> : IFactoryBehaviorMetadata<TB, T>
        where TB : ITvBehavior<T>
    {
        private Action<TB> _afterCreate;
        private BehaviorSchedule _schedule;
        private ITvBehavior<T> _behavior;
        private List<ComponentDependencyDescriptor> _dependencies;
        private Func<IServiceProvider, TB> _creator;

        public FactoryBehaviorMetadata()
        {
            _creator = DefaultCreator;
            _dependencies = new List<ComponentDependencyDescriptor>();
        }

        ITvBehavior<T> IBehaviorMetadata<T>.Behavior { get => _behavior; }

        ITvBehavior IBehaviorMetadata.Behavior { get => _behavior; }

        bool IBehaviorMetadata.Created { get => _behavior != null; }

        BehaviorSchedule IBehaviorMetadata.Schedule { get => _schedule; }

        void IBehaviorMetadata.CreateBehavior(IServiceProvider sp)
        {
            if (_behavior is null)
            {
                var behavior = _creator(sp);
                _afterCreate?.Invoke((TB)behavior);
                _behavior = behavior;
            }
        }

        IEnumerable<OwnedComponentDependencyDescriptor> IBehaviorMetadata.Dependencies
        {
            get
            {
                foreach (var dep in _dependencies)
                {
                    yield return new OwnedComponentDependencyDescriptor(_behavior, dep.Attribute, dep.Property);
                }
            }
        }


        public IBehaviorMetadata<T> UseScheduler(BehaviorSchedule schedule)
        {
            _schedule = schedule;
            return this;
        }

        public void OnCreate(Action<TB> afterCreate)
        {
            _afterCreate = afterCreate;
        }

        public void CreateUsing(Func<IServiceProvider, TB> creator)
        {
            _creator = creator;
        }

        public void AddDependency<TR>(Expression<Func<TB, TR>> propertyExp, string name)
        {
            var member = propertyExp.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException("property expression must be a property");
            }

            var property = member.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("property expression must be a property");
            }

            var attr = new TvComponentDependencyAttribute()
            {
                Name = name
            };

            _dependencies.Add(new ComponentDependencyDescriptor(attr, property));
        }

        private static TB DefaultCreator(IServiceProvider sp)
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
            var behavior = (TB)ctor.Invoke(parameters.ToArray());
            return behavior;

        }

    }
}
