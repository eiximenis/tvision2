using System;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    internal class TvComponentMetadata : IComponentMetadata
    {
        public TvComponent Component { get; }

        public event EventHandler<ViewportUpdatedEventArgs> ViewportChanged;

        public TvComponentMetadata(TvComponent component)
        {
            Component = component;
        }

        public void OnViewportChanged(Guid id, IViewport previous, IViewport current)
        {
            var handler = ViewportChanged;
            handler?.Invoke(this, new ViewportUpdatedEventArgs(id, previous, current));
        }
    }

}
