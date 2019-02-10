using System;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    internal class TvComponentMetadata : IComponentMetadata, IConfigurableComponentMetadata
    {
        public TvComponent Component { get; }

        public event EventHandler<ViewportUpdatedEventArgs> ViewportChanged;

        internal Action<TvComponent, IComponentTree> MountAction { get; private set; }
        internal Action<TvComponent, IComponentTree> UnmountAction { get; private set; }

        public TvComponentMetadata(TvComponent component)
        {
            Component = component;
        }

        public void OnViewportChanged(Guid id, IViewport previous, IViewport current)
        {
            var handler = ViewportChanged;
            handler?.Invoke(this, new ViewportUpdatedEventArgs(id, previous, current, Component.Name));
        }

        public void WhenComponentMounted(Action<TvComponent, IComponentTree> mountAction)
        {
            MountAction = mountAction;
        }

        public void WhenComponentUnmounted(Action<TvComponent, IComponentTree> unmountAction)
        {
            UnmountAction = unmountAction;
        }
    }

}
