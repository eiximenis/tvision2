using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Core.Engine
{

    public interface IReadonlyComponentTree
    {
        TvComponent GetComponent(string name);
        IEnumerable<TvComponent> Components { get; }

    }

    public interface IComponentTree : IReadonlyComponentTree
    {
        event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        IComponentMetadata AddAfter(TvComponent componentToAdd, TvComponent componentBefore, Action<ITuiEngine> afterAddAction = null);
        IComponentMetadata Add(TvComponent component, Action<ITuiEngine> afterAddAction = null);
        IComponentMetadata AddAsChild(TvComponent componentToAdd, TvComponent parent, Action<ITuiEngine> afterAddAction = null);

        bool Remove(TvComponent component);

        void Clear();
    }
}
