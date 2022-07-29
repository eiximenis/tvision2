using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Stack
{





    public class TvStackPanel : ITvContainer, ICellContainer
    {
        private readonly TvComponent<StackLayout> _thisComponent;
        private readonly KeyedComponentsCollection<int> _childs;

        private readonly List<TvComponent> _pendingAdds;
        private bool _isMounted;

        private IComponentTree _tree;
        public string Name { get; }

        public StackLayout Layout => _thisComponent.State;

        private ChildAlignment _currentAlignment;

        private int _currentKey;

        public TvStackPanel(string name = null)
        {
            _currentKey = 0;
            _currentAlignment = ChildAlignment.None;
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

        private void OnChildUnmounted(ChildComponentUnmoutingContext ctx)
        {
            if (!ctx.ParentIsUnmountedToo)
            {
                RepositionChildren(TvPoint.Zero);
            }
        }

        private void OnChildMounted(ChildComponentMoutingContext ctx)
        {
            RepositionChildren(TvPoint.Zero);
        }

        public ICellContainer At(int idx)
        {
            var layout = _thisComponent.State;
            if (layout.ItemsCount > idx)
            {
                _currentKey = idx;
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
                var rowsUsed = 0;
                foreach (var kvpChild in _childs.Values.OrderBy(k => k.Key))
                {
                    var layoutIdx = kvpChild.Key;
                    var childComponent = kvpChild.Value.Component;
                    var height = Layout.GetRealSize(Layout[layoutIdx], _thisComponent.Viewport.Bounds.Rows);
                    var newViewport = _thisComponent.Viewport.TakeRows(height, rowsUsed);
                    childComponent.UpdateViewport(newViewport, addIfNotExists: true);
                    rowsUsed += height;
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



        void ICellContainer.Add(TvComponent child)
        {
            _childs.Set(_currentKey, child, _currentAlignment);
            if (_isMounted)
            {
                _tree.AddAsChild(child, AsComponent());
            }
            else
            {
                _pendingAdds.Add(child);
            }
            _currentAlignment = ChildAlignment.None;
        }

        ICellContainer ICellContainer.WithAlignment(ChildAlignment alignment)
        {
            _currentAlignment = alignment;
            return this;
        }

        TvComponent? ICellContainer.Get() => _childs.Get(_currentKey)?.Component;
    }
}
