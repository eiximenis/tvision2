using System;
using System.Linq;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    internal class ViewportManager : IViewportManager
    {
        private ComponentTree _attachedComponentTree;
        public void InvalidateViewport(IViewport viewportToInvalidate)
        {
            var components = _attachedComponentTree.Components.Where(c => c.Viewports.Any(vp => vp.Intersects(viewportToInvalidate)));
            foreach (var component in components)
            {
                component.Invalidate();
            }
        }

        public void AttachTo(ComponentTree componentTree)
        {
            _attachedComponentTree = componentTree;

            foreach (var component in componentTree.Components)
            {
                component.Invalidate();
                component.Metadata.ViewportChanged += OnViewportChanged;
            }

            componentTree.ComponentAdded += OnComponentAdded;
            componentTree.ComponentRemoved += OnComponentRemoved;
        }

        private void OnComponentRemoved(object sender, TreeUpdatedEventArgs e)
        {
            var removed = e.ComponentMetadata;
            var viewport = e.ComponentMetadata.Component.Viewport;
            if (viewport != null)
            {
                InvalidateViewport(viewport);
            }
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
            _attachedComponentTree.ClearViewport(e.Previous);
            data.Component.Invalidate();
            InvalidateViewport(e.Previous);
        }
    }
}