using System;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts
{
    public class TvContainer : ITvContainer
    {
        private readonly TvComponent<LayoutsComponentTree> _thisComponent;
        private readonly LayoutsComponentTree _childs;
        public string Name { get; }

        public TvContainer(IComponentTree root, string name = null)
        {
            _childs = new LayoutsComponentTree(root);
            _thisComponent = new TvComponent<LayoutsComponentTree>(_childs, name ?? $"TvContainer{Guid.NewGuid()}");
            Name = _thisComponent.Name;
            _thisComponent.Metadata.ViewportChanged += OnViewportChange;
            _thisComponent.AddDrawer(ct => { });
        }

        public void AddChild(TvComponent child)
        {
            _childs.Add(child);
            InsertChildInside(child);
            RepositionChildren(TvPoint.Zero);
        }

        private void InsertChildInside(TvComponent child)
        {
            if (_thisComponent.Viewport != null)
            {
                child.UpdateViewport(_thisComponent.Viewport.Inner(child.Viewport), addIfNotExists: true);
            }
        }

        protected virtual void RepositionChildren(TvPoint displacement)
        {
            if (_thisComponent.Viewport != null)
            {
                foreach (var child in _childs.Components)
                {
                    var childvp = child.Viewport;
                    child.UpdateViewport(childvp.Translate(displacement));
                }
            }
        }

        private void OnViewportChange(object sender, ViewportUpdatedEventArgs e)
        {
            if (e.Previous == Viewport.NullViewport)
            {
                foreach (var child in _childs.Components)
                {
                    InsertChildInside(child);
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
