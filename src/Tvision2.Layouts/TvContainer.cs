using System;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts
{
    public class TvContainer : ITvContainer
    {
        private readonly TvComponent<IComponentTree> _thisComponent;
        private readonly LayoutsComponentTree _childs;
        public TvContainer(ComponentTree root, string name = null)
        {
            _childs = new LayoutsComponentTree(root);
            _thisComponent = new TvComponent<IComponentTree>(_childs, name);
            _thisComponent.Metadata.ViewportChanged += OnChildViewportChanged;
            _thisComponent.UseDrawer(ct => { });
        }

        public void AddChild(TvComponent child)
        {
            _childs.Add(child);
            RecalculateChildren();
        }

        protected virtual void RecalculateChildren()
        {
            if (_thisComponent.Viewport != null)
            {
                foreach (var child in _childs.Components)
                {
                    child.UpdateViewport(_thisComponent.Viewport.Inner(child.Viewport), addIfNotExists: true);
                }
            }
        }

        private void OnChildViewportChanged(object sender, EventArgs e)
        {
            RecalculateChildren();
        }

        public TvComponent AsComponent() => _thisComponent;
    }
}
