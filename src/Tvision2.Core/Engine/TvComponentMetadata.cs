using System;
using System.Threading.Tasks;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    internal class TvComponentMetadata : IComponentMetadata, IConfigurableComponentMetadata
    {
        public TvComponent Component { get; }

        public event EventHandler<ViewportUpdatedEventArgs> ViewportChanged;

        public IActionChain<ComponentMoutingContext> OnComponentMounted { get; }
        public IActionChain<ComponentMoutingContext> OnComponentUnmounted { get; }

        public TvComponentMetadata(TvComponent component)
        {
            Component = component;
            OnComponentMounted = new ActionChain<ComponentMoutingContext>();
            OnComponentUnmounted = new ActionChain<ComponentMoutingContext>();
        }

        public void OnViewportChanged(Guid id, IViewport previous, IViewport current)
        {
            var handler = ViewportChanged;
            handler?.Invoke(this, new ViewportUpdatedEventArgs(id, previous, current, Component.Name));
        }

        public void WhenComponentMounted(Func<ComponentMoutingContext, Task<bool>> mountAction)
        {
            OnComponentMounted.Add(mountAction);
        }

        public void WhenComponentUnmounted(Func<ComponentMoutingContext, Task<bool>> unmountAction)
        {
            OnComponentUnmounted.Add(unmountAction);
        }
    }

}
