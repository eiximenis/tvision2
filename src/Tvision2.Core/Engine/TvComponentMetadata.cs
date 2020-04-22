﻿using System;
using System.Threading.Tasks;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public class TvComponentMetadata : IConfigurableComponentMetadata
    {
        public TvComponent Component { get; }

        public Guid Id
        {
            get => Component.ComponentId;
        }

        public bool IsMounted { get; private set; }
        public bool PropagateStatusToChildren { get; private set; }
        public bool AdmitStatusPropagation { get; private set; }

        public event EventHandler<ViewportUpdatedEventArgs> ViewportChanged;
        private readonly ActionChain<ComponentMoutingContext> _onComponentMounted;
        private readonly ActionChain<ComponentMoutingContext> _onComponentUnmounted;
        private readonly ActionChain<ComponentMountingCancellableContext> _onComponentWillBeUnmounted;
        private readonly ActionChain<ChildComponentMoutingContext> _onChildMounted;
        private readonly ActionChain<ChildComponentMoutingContext> _onChildUnmounted;


        public IActionChain<ComponentMoutingContext> OnComponentMounted => _onComponentMounted;
        public IActionChain<ComponentMoutingContext> OnComponentUnmounted => _onComponentUnmounted;
        public IActionChain<ComponentMountingCancellableContext> OnComponentWillBeUnmounted => _onComponentWillBeUnmounted;
        public IActionChain<ChildComponentMoutingContext> OnChildMounted => _onChildMounted;
        public IActionChain<ChildComponentMoutingContext> OnChildUnmounted => _onChildUnmounted;

        internal TvComponentMetadata(TvComponent component)
        {
            Component = component;
            _onComponentMounted = new ActionChain<ComponentMoutingContext>();
            _onComponentUnmounted = new ActionChain<ComponentMoutingContext>();
            _onComponentWillBeUnmounted = new ActionChain<ComponentMountingCancellableContext>();
            _onChildMounted = new ActionChain<ChildComponentMoutingContext>();
            _onChildUnmounted = new ActionChain<ChildComponentMoutingContext>();
        }

        internal ComponentTreeNode TreeNode { get; private set; }

        internal void OnViewportChanged(Guid id, IViewport previous, IViewport current)
        {
            var handler = ViewportChanged;
            handler?.Invoke(this, new ViewportUpdatedEventArgs(id, previous, current, Component.Name));
        }

        void IConfigurableComponentMetadata.WhenComponentMounted(Action<ComponentMoutingContext> mountAction)
        {
            _onComponentMounted.Add(mountAction);
        }

        void IConfigurableComponentMetadata.WhenComponentUnmounted(Action<ComponentMoutingContext> unmountAction)
        {
            _onComponentUnmounted.Add(unmountAction);
        }

        void IConfigurableComponentMetadata.WhenComponentWillbeUnmounted(Action<ComponentMountingCancellableContext> unmountAction)
        {
            _onComponentWillBeUnmounted.Add(unmountAction);
        }

        void IConfigurableComponentMetadata.WhenChildMounted(Action<ChildComponentMoutingContext> childMountAction)
        {
            _onChildMounted.Add(childMountAction);
        }

        void IConfigurableComponentMetadata.WhenChildUnmounted(Action<ChildComponentMoutingContext> childUndmountAction)
        {
            _onChildUnmounted.Add(childUndmountAction);
        }

        internal bool CanBeUnmountedFrom(ITuiEngine ownerEngine)
        {
            var ctx = new ComponentMountingCancellableContext(ownerEngine, this.Component);
            _onComponentWillBeUnmounted.Invoke(ctx);
            return !ctx.IsCancelled;
        }

        internal void MountedTo(ITuiEngine ownerEngine, ComponentTree tree, ComponentTreeNode node, AddComponentOptions optionsUsed)
        {
            IsMounted = true;
            AdmitStatusPropagation = optionsUsed.RetrieveParentStatusChanges;
            TreeNode = node;
            _onComponentMounted.Invoke(new ComponentMoutingContext(ownerEngine, tree, this.Component, node));
        }

        internal void UnmountedFrom(ITuiEngine ownerEngine, ComponentTree tree)
        {
            IsMounted = false;
            TreeNode = null;
            _onComponentUnmounted.Invoke(new ComponentMoutingContext(ownerEngine, tree, this.Component, null));
        }

        internal void OnChildAdded(TvComponentMetadata childMetadata, ComponentTreeNode childNode, AddComponentOptions childAddOptions)
        {
            if (childMetadata.AdmitStatusPropagation)
            {
                PropagateStatusToChildren = true;
            }

            _onChildMounted.Invoke(new ChildComponentMoutingContext(childMetadata.Component, childNode));
        }
        internal void OnChildRemoved(TvComponentMetadata childMetadata, ComponentTreeNode nodeRemoved)
        {
            _onChildUnmounted.Invoke(new ChildComponentMoutingContext(childMetadata.Component, nodeRemoved));
        }

    }

}
