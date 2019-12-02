using System;
using Tvision2.Core.Components;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public interface IComponentMetadata
    {
        bool IsMounted { get; }
        TvComponent Component { get; }
        event EventHandler<ViewportUpdatedEventArgs> ViewportChanged;
        IActionChain<ComponentMoutingContext> OnComponentMounted { get; }
        IActionChain<ComponentMoutingContext> OnComponentUnmounted { get; }
        IActionChain<ComponentMountingCancellableContext> OnComponentWillBeUnmounted { get; }
    }
}
