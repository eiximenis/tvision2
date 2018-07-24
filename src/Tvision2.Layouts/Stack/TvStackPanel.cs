using System;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Stack
{
    public class TvStackPanel : ITvContainer
    {
        private readonly TvComponent<LayoutsComponentTree> _thisComponent;
        private readonly LayoutsComponentTree _childs;
        public string Name { get; }

        public IComponentTree Children => _childs;

        public TvStackPanel(IComponentTree root, string name = null)
        {
            _childs = new LayoutsComponentTree(root);
            _thisComponent = new TvComponent<LayoutsComponentTree>(_childs, name ?? $"TvContainer{Guid.NewGuid()}");
            Name = _thisComponent.Name;
            _thisComponent.Metadata.ViewportChanged += OnViewportChange;
            _childs.ComponentAdded += OnChildAdded;
            _childs.ComponentRemoved += OnChildRemoved;
        }

        private void OnChildRemoved(object sender, TreeUpdatedEventArgs e)
        {
            RepositionChildren(TvPoint.Zero);
        }

        private void OnChildAdded(object sender, TreeUpdatedEventArgs e)
        {
            var child = e.ComponentMetadata.Component;
            InsertChildInside(child);
            RepositionChildren(TvPoint.Zero);
        }


        private void InsertChildInside(TvComponent child)
        {
            //if (_thisComponent.Viewport != null)
            //{
            //    child.UpdateViewport(_thisComponent.Viewport.Inner(child.Viewport), addIfNotExists: true);
            //}
        }


        private void RepositionChildren(TvPoint displacement)
        {
            if (_thisComponent.Viewport != null)
            {
                var current = 0;
                var height = _thisComponent.Viewport.Rows / _childs.Count;
                foreach (var child in _childs.Components)
                {
                    var childvp = child.Viewport;
                    child.UpdateViewport(_thisComponent.Viewport.TakeRows(height, height * current));
                }
            }
        }

        private void OnViewportChange(object sender, ViewportUpdatedEventArgs e)
        {
            if (e.Previous == Viewport.NullViewport)
            {
                foreach (var child in _childs.Components)
                {
                    RepositionChildren(TvPoint.Zero);
                }
            }
            else
            {
                var displacement = e.Current.Position - e.Previous.Position;
                RepositionChildren(displacement);
            }
        }

        public TvComponent AsComponent() => _thisComponent;
    }
}
