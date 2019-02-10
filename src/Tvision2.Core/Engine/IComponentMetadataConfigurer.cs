using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public interface IConfigurableComponentMetadata
    {
        void WhenComponentMounted(Action<TvComponent, IComponentTree> mountAction);
        void WhenComponentUnmounted(Action<TvComponent, IComponentTree> unmountAction);
    }
}
