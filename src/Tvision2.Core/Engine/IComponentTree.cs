using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Core.Engine
{
    public interface IComponentTree 
    {
        event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;
        TuiEngine Engine { get; }
        IComponentMetadata Add(TvComponent component, Action afterAddAction = null);
        bool Remove(TvComponent component);
        TvComponent GetComponent(string name);
        IEnumerable<TvComponent> Components { get; }

        void Clear();
    }
}
