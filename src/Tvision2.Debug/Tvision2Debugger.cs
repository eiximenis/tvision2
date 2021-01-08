    using System.Linq;
using Tvision2.Core.Engine;

namespace Tvision2.Debug
{
    internal class Tvision2Debugger : ITvision2Debugger
    {

        private readonly Tvision2DebugOptions _options;
        public Tvision2Debugger(Tvision2DebugOptions options) => _options = options;

        public void AttachTo(IComponentTree componentTree)
        {
            foreach (var component in componentTree.Components.Where(c => _options.ComponentDebuggable(c)))
            {
                component.InsertDrawerAt(new DebugDrawer(), 0);
            }
            componentTree.ComponentAdded += OnComponentAdded;
        }

        private void OnComponentAdded(object sender, TreeUpdatedEventArgs e)
        {
            var component = e.ComponentMetadata.Component;
            if (_options.ComponentDebuggable(component))
            {
                component.InsertDrawerAt(new DebugDrawer(), 0);
            }
        }
    }
}