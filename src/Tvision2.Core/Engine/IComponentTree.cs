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

        IEnumerable<ComponentTreeNode> NodesList { get; }

    }

    public interface IComponentTree : IReadonlyComponentTree
    {
        event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;
        event EventHandler TreeUpdated;

        IOnceActionChain<IComponentTree> OnAddingIdleCycle { get; }

        TvComponentMetadata Add(TvComponent component, Action<AddComponentOptions>? addOptions = null);
        TvComponentMetadata AddAfter(TvComponent componentToAdd, TvComponent componentBefore);
        TvComponentMetadata AddAsChild(TvComponent componentToAdd, TvComponent parent, Action<IAddChildComponentOptions>? options = null);
        bool Remove(TvComponent component);

        void Clear();
    }
}
