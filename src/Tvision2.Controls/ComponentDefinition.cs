using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Draw;

namespace Tvision2.Controls
{
    public class ComponentDefinition : IComponentDefinition
    {
        private readonly List<ITvBehavior> _behaviors;
        private readonly List<ITvDrawer> _drawers;
        public IEnumerable<ITvBehavior> Behaviors => _behaviors;
        public IEnumerable<ITvDrawer> Drawers => _drawers;

        public ComponentDefinition()
        {
            _behaviors = new List<ITvBehavior>();
            _drawers = new List<ITvDrawer>();
        }

        public void AddDrawer(ITvDrawer drawer)
        {
            _drawers.Add(drawer);
        }

        public void AddBehavior(ITvBehavior behavior)
        {
            _behaviors.Add(behavior);
        }


    }
}
