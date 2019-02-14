using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public interface IConfigurableComponentMetadata
    {
        void WhenComponentMounted(Func<ComponentMoutingContext, Task<bool>> mountAction);
        void WhenComponentUnmounted(Func<ComponentMoutingContext, Task<bool>> unmountAction);
    }
}
