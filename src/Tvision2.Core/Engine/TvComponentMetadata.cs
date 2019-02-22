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
        private readonly ActionChain<ComponentMoutingContext> _onComponentMounted;
        private readonly ActionChain<ComponentMoutingContext> _onComponentUnmounted;
        private readonly ActionChain<ComponentMountingCancellableContext> _onComponentWillBeUnmounted;

        public ITuiEngine Engine { get; private set; }
        public bool IsMounted { get; private set; }


        public IActionChain<ComponentMoutingContext> OnComponentMounted => _onComponentMounted;
        public IActionChain<ComponentMoutingContext> OnComponentUnmounted => _onComponentUnmounted;
        public IActionChain<ComponentMountingCancellableContext> OnComponentWillBeUnmounted => _onComponentWillBeUnmounted;

        public TvComponentMetadata(TvComponent component)
        {
            Component = component;
            _onComponentMounted = new ActionChain<ComponentMoutingContext>();
            _onComponentUnmounted = new ActionChain<ComponentMoutingContext>();
            _onComponentWillBeUnmounted = new ActionChain<ComponentMountingCancellableContext>();
        }

        public void OnViewportChanged(Guid id, IViewport previous, IViewport current)
        {
            var handler = ViewportChanged;
            handler?.Invoke(this, new ViewportUpdatedEventArgs(id, previous, current, Component.Name));
        }

        public void WhenComponentMounted(Action<ComponentMoutingContext> mountAction)
        {
            _onComponentMounted.Add(mountAction);
        }

        public void WhenComponentUnmounted(Action<ComponentMoutingContext> unmountAction)
        {
            _onComponentUnmounted.Add(unmountAction);
        }

        public void WhenComponentWillbeUnmounted(Action<ComponentMountingCancellableContext> unmountAction)
        {
            _onComponentWillBeUnmounted.Add(unmountAction);
        }

        internal bool CanBeUnmountedFrom(ComponentTree owner)
        {
            var ctx = new ComponentMountingCancellableContext(owner, this.Component);
            _onComponentWillBeUnmounted.Invoke(ctx);
            return !ctx.IsCancelled;
        }

        internal void MountedTo(IComponentTree owner)
        {
            Engine = owner.Engine;
            IsMounted = true;
            _onComponentMounted.Invoke(new ComponentMoutingContext(owner, this.Component));
        }

        internal void UnmountedFrom(ComponentTree owner)
        {
            Engine = null;
            IsMounted = false;
            _onComponentUnmounted.Invoke(new ComponentMoutingContext(owner, this.Component));
        }


    }

}
