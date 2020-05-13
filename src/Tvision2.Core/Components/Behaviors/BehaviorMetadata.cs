using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Tvision2.Core.Engine.ComponentTree;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorMetadata<T, TB> : IBehaviorMetadata<T>
         where TB : ITvBehavior<T>
    {
        private readonly ITvBehavior<T> _behavior;
        private BehaviorSchedule _schedule;
        private List<ComponentDependencyDescriptor> _dependencies;

        ITvBehavior<T> IBehaviorMetadata<T>.Behavior { get => _behavior; }

        ITvBehavior IBehaviorMetadata.Behavior { get => _behavior; }

        BehaviorSchedule IBehaviorMetadata.Schedule { get => _schedule; }

        bool IBehaviorMetadata.Created { get => true; }

        void IBehaviorMetadata.CreateBehavior(IServiceProvider sp) { }          // No need to do anything as behavior is always creeated.

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

        public void UseScheduler(BehaviorSchedule schedule)
        {
            _schedule = schedule;
        }

        public BehaviorMetadata(TB behavior)
        {
            _dependencies = new List<ComponentDependencyDescriptor>();
            _behavior = behavior;
            _schedule = BehaviorSchedule.OncePerFrame;
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
    }

}
