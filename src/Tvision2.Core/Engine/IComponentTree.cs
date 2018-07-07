using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Core.Engine
{
    public interface IComponentTree
    {
        IEnumerable<TvComponent> Components { get; }
        event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
    }
}
