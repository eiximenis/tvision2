using System;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Stack
{
    public class TvStackPanel : ITvContainer
    {
        private readonly TvComponent<StackLayout> _thisComponent;
        private readonly KeyedComponentsTree<int> _childs;
        public string Name { get; }

        public StackLayout Layout => _thisComponent.State;

        public TvStackPanel(IComponentTree root, string name = null)
        {
            _childs = new KeyedComponentsTree<int>(root);
            _thisComponent = new TvComponent<StackLayout>(new StackLayout(), name ?? $"TvStackPanel_{Guid.NewGuid()}");
            Name = _thisComponent.Name;
            _thisComponent.Metadata.ViewportChanged += OnViewportChange;
            _childs.ComponentAdded += OnChildAdded;
            _childs.ComponentRemoved += OnChildRemoved;
        }

        public void Clear()
        {
            var layout = _thisComponent.State;
            for (var idx=0; idx < layout.ItemsCount; idx++)
            {
                At(idx).Clear();
            }
        }

        private void OnChildRemoved(object sender, TreeUpdatedEventArgs e)
        {
            RepositionChildren(TvPoint.Zero);
        }

        public IComponentTree At (int idx)
        {
            var layout = _thisComponent.State;
            if (layout.ItemsCount > idx)
            {
                _childs.SetKey(idx);
                return _childs;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(idx), idx, $"Value {idx} out of range 0..{layout.ItemsCount -1 }");
            }

        }

        private void OnChildAdded(object sender, TreeUpdatedEventArgs e)
        {
            var child = e.ComponentMetadata.Component;
            RepositionChildren(TvPoint.Zero);
        }


        private void RepositionChildren(TvPoint displacement)
        {
            if (_thisComponent.Viewport != null)
            {
                var layout = _thisComponent.State;
                if (layout.ItemsCount > 0)
                {
                    for (var layoutIdx = 0; layoutIdx < layout.ItemsCount; layoutIdx++)
                    {
                        var height = Layout.GetRealSize(Layout[layoutIdx], _thisComponent.Viewport.Rows);
                        var itemChilds = _childs.ComponentsForKey(layoutIdx);
                        foreach (var childComponent in itemChilds)
                        {
                            var childvp = childComponent.Viewport;
                            childComponent.UpdateViewport(_thisComponent.Viewport.TakeRows(height, height * layoutIdx), addIfNotExists: true);
                        }

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
    }
}
