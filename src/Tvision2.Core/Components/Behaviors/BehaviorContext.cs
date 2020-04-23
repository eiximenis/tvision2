using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public readonly ref struct BehaviorContext<T>
    {
        public IViewport Viewport { get; }

        public ITvConsoleEvents Events { get; }
        public T State { get; }

        private readonly ComponentTreeNode _parent;

        public BehaviorContext(T state, ITvConsoleEvents events, IViewport viewport, ComponentTreeNode parent)
        {
            Events = events;
            Viewport = viewport;
            State = state;
            _parent = parent;
        }

        public TRootState GetRootState<TRootState>() =>
            ((TvComponent<TRootState>)_parent.Root.Data.Component).State;

        public TParentState GetParentState<TParentState>() =>
            ((TvComponent<TParentState>)_parent.Data.Component).State;

        public bool ComponentHasParent => _parent != null;


    }
}
