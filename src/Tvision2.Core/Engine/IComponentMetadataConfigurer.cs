using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public interface IConfigurableComponentMetadata
    {
        void WhenComponentMounted(Action<ComponentMoutingContext> mountAction);
        void WhenComponentUnmounted(Action<ComponentMoutingContext> unmountAction);
        void WhenComponentWillbeUnmounted(Action<ComponentMountingCancellableContext> unmountAction);
        void WhenChildMounted(Action<ChildComponentMoutingContext> childMountAction);
        void WhenChildUnmounted(Action<ChildComponentUnmoutingContext> childUndmountAction);
        void WhenComponentTreeUpdatedByMount(Action<ComponentTreeUpdatedContext> componentTreeAction);
    }
}
