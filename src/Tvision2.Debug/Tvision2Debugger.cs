using Tvision2.Core.Engine;

namespace Tvision2.Debug
{
    internal class Tvision2Debugger : ITvision2Debugger
    {
        public void AttachTo(ComponentTree componentTree)
        {
            foreach (var component in componentTree.Components)
            {
                if (component.Name.StartsWith("TvComponent"))
                {
                    component.InsertDrawerAt(new DebugDrawer(), 0);
                }
            }
            componentTree.ComponentAdded += OnComponentAdded;
        }

        private void OnComponentAdded(object sender, TreeUpdatedEventArgs e)
        {
            var component = e.ComponentMetadata.Component;
            if (component.Name.StartsWith("TvComponent"))
            {

                component.InsertDrawerAt(new DebugDrawer(), 0);
            }
        }
    }
}