using System;
using System.Linq;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    internal class ViewportManager : IViewportManager
    {
        private IComponentTree _attachedComponentTree;
        public void InvalidateViewport(IViewport viewportToInvalidate)
        {
            var components = _attachedComponentTree.Components.Where(c => c.Viewports.Any(vp => vp.Intersects(viewportToInvalidate)));
            foreach (var component in components)
            {
                component.Invalidate();
            }
        }

        public void AttachTo(IComponentTree componentTree)
        {
            _attachedComponentTree = componentTree;

            foreach (var component in componentTree.Components)
            {
                component.Invalidate();
                component.Metadata.ViewportChanged += OnViewportChanged;
            }

            componentTree.ComponentAdded += OnComponentAdded;
        }

        private void OnComponentAdded(object sender, TreeUpdatedEventArgs e)
        {
            var data = e.ComponentMetadata;
            data.ViewportChanged += OnViewportChanged;
        }

        private void OnViewportChanged(object sender, ViewportUpdatedEventArgs e)
        {
            // TODO: process viewportchange
            var data = sender as IComponentMetadata;
            data.Component.Invalidate();
        }
    }
}