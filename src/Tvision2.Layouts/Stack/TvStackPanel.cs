using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Stack
{
    public class TvStackPanel : ITvContainer, IComponentsCollection
    {
        private readonly TvComponent<StackLayout> _thisComponent;
        private readonly KeyedComponentsCollection<int> _childs;

        private readonly List<TvComponent> _pendingAdds;
        private bool _isMounted;

        private IComponentTree _tree;
        public string Name { get; }

        public StackLayout Layout => _thisComponent.State;

        int IComponentsCollection.Count => throw new NotImplementedException();

        public TvStackPanel(string name = null)
        {
            _childs = new KeyedComponentsCollection<int>();
            _pendingAdds = new List<TvComponent>();
            _isMounted = false;
            _thisComponent = new TvComponent<StackLayout>(new StackLayout(), name ?? $"TvStackPanel_{Guid.NewGuid()}",
                cfg =>
                {
                    cfg.WhenChildMounted(ctx => OnChildMounted(ctx));
                    cfg.WhenChildUnmounted(ctx => OnChildUnmounted(ctx));
                    cfg.WhenComponentMounted(ctx => OnComponentMounted(ctx));
                });
            Name = _thisComponent.Name;
          
        }

        private void OnComponentMounted(ComponentMoutingContext ctx)
        {
            _tree = ctx.ComponentTree;
            _isMounted = true;

            if (_pendingAdds.Any())
            {
                foreach (var cmpToAdd in _pendingAdds)
                {
                    DoPendingAdd(cmpToAdd);
                }
                _pendingAdds.Clear();
                RepositionChildren(TvPoint.Zero);
            }
        }

        private void OnChildUnmounted(ChildComponentMoutingContext ctx)
        {
            RepositionChildren(TvPoint.Zero);
        }

        private void OnChildMounted(ChildComponentMoutingContext ctx)
        {
            RepositionChildren(TvPoint.Zero);
        }

        public IComponentsCollection At(int idx)
        {
            var layout = _thisComponent.State;
            if (layout.ItemsCount > idx)
            {
                _childs.SetKey(idx);
                return this;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(idx), idx, $"Value {idx} out of range 0..{layout.ItemsCount - 1 }");
            }

        }


        private void RepositionChildren(TvPoint displacement)
        {
            if (_thisComponent.Viewport != null)
            {
                var layout = _thisComponent.State;
                var rowsUsed = 0;
                if (layout.ItemsCount > 0)
                {
                    for (var layoutIdx = 0; layoutIdx < layout.ItemsCount; layoutIdx++)
                    {
                        var height = Layout.GetRealSize(Layout[layoutIdx], _thisComponent.Viewport.Bounds.Rows);
                        var itemChilds = _childs.ComponentsForKey(layoutIdx);
                        foreach (var childComponent in itemChilds)
                        {
                            var childvp = childComponent.Viewport;
                            childComponent.UpdateViewport(_thisComponent.Viewport.TakeRows(height, rowsUsed), addIfNotExists: true);
                        }
                        rowsUsed += height;

                    }
                }
            }
        }

        private void OnViewportChange(object sender, ViewportUpdatedEventArgs e)
        {
            if (e.Previous == Viewport.NullViewport)
            {
                RepositionChildren(TvPoint.Zero);
            }
            else
            {
                var displacement = e.Current.Position - e.Previous.Position;
                RepositionChildren(displacement);
            }
        }



        public TvComponent AsComponent() => _thisComponent;

        public void Clear() => _childs.Clear();


        private void DoPendingAdd(TvComponent cmpToAdd)
        {
            _tree.AddAsChild(cmpToAdd, AsComponent(), cfg => cfg.DoNotNotifyParentOnAdd());
        }

        void IComponentsCollection.Add(TvComponent child)
        {
            _childs.Add(child);
            if (_isMounted)
            {
                _tree.AddAsChild(child, AsComponent());
            }
            else
            {
                _pendingAdds.Add(child);
            }
        }

        bool IComponentsCollection.Remove(TvComponent component) => _childs.Remove(component);

        IEnumerator<TvComponent> IEnumerable<TvComponent>.GetEnumerator() => _childs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _childs.GetEnumerator();
    }
}
