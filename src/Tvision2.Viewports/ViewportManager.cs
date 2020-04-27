using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    internal class ViewportManager : IViewportManager
    {
        private ComponentTree _attachedComponentTree;

        private readonly List<IViewport> _viewportsToInvalidate;

        public ViewportManager()
        {
            _viewportsToInvalidate = new List<IViewport>();
        }

        public void InvalidateViewport(IViewport viewportToInvalidate)
        {
            var components = _attachedComponentTree.Components.Where(c => c.Viewports.Any(vp => vp.Value.Intersects(viewportToInvalidate)));
            foreach (var component in components)
            {
                component.Invalidate(InvalidateReason.FullDrawRequired);
            }
        }

        public void AttachTo(ComponentTree componentTree)
        {
            _attachedComponentTree = componentTree;

            foreach (var component in componentTree.Components)
            {
                component.Invalidate(InvalidateReason.FullDrawRequired);
                component.Metadata.ViewportChanged += OnViewportChanged;
            }

            componentTree.ComponentAdded += OnComponentAdded;
            componentTree.ComponentRemoved += OnComponentRemoved;
            componentTree.TreeUpdated += OnTreeUpdated;
        }


        private void OnComponentRemoved(object sender, TreeUpdatedEventArgs e)
        {
            e.ComponentMetadata.ViewportChanged -= OnViewportChanged;
            _viewportsToInvalidate.AddRange(e.ComponentMetadata.Component.Viewports.Select(v => v.Value));
        }

        private void OnTreeUpdated(object sender, EventArgs e)
        {
            if (_viewportsToInvalidate.Count != 0)
            {
                foreach (var viewport in _viewportsToInvalidate)
                {
                    if (viewport != null)
                    {
                        InvalidateViewport(viewport);
                    }
                }

                _viewportsToInvalidate.Clear();
            }
        }

        private void OnComponentAdded(object sender, TreeUpdatedEventArgs e)
        {
            var data = e.ComponentMetadata;
            data.ViewportChanged += OnViewportChanged;
        }

        private void OnViewportChanged(object sender, ViewportUpdatedEventArgs e)
        {
            var data = sender as TvComponentMetadata;
            _attachedComponentTree.ClearViewport(e.Previous);
            data.Component.Invalidate(InvalidateReason.FullDrawRequired);
            InvalidateViewport(e.Previous);
        }
    }
}