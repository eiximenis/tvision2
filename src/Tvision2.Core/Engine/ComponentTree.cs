using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Cryptography;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Core.Engine
{

    public class ComponentTreeNode
    {

        public struct RemoveResult
        {
            private readonly List<ComponentTreeNode> _allDeletedNodes;
            public RemoveStatus Status { get; }
            public ComponentTreeNode NearestParent { get; }

            public IEnumerable<ComponentTreeNode> AllDeletedNodes { get { return _allDeletedNodes ?? Enumerable.Empty<ComponentTreeNode>(); } }

            public RemoveResult(RemoveStatus status, ComponentTreeNode nearestParent, List<ComponentTreeNode> deletedNodes)
            {
                Status = status;
                NearestParent = nearestParent;
                _allDeletedNodes = deletedNodes;
            }

            public static RemoveResult NotFound() => new RemoveResult(RemoveStatus.NotFound, null, null);
        }

        public enum RemoveStatus
        {
            NotFound = 0,
            RootDeleted = 1,
            ChildDeleted = 2
        }


        public bool HasParent
        {
            get => Parent != null;
        }
        public ComponentTreeNode Parent { get; private set; }

        private Dictionary<Guid, ComponentTreeNode> _childs;
        private readonly Dictionary<Type, object> _tags;
        public bool IsDismissed { get; private set; }

        public TvComponentMetadata Data { get; private set; }

        public int Level { get; private set; }

        public IEnumerable<ComponentTreeNode> Childs
        {
            get => _childs.Values;
        }


        public IEnumerable<ComponentTreeNode> Descendants()
        {
            return _childs.SelectMany(c => c.Value.SubTree());
        }
        public IEnumerable<ComponentTreeNode> SubTree()
        {
            return _childs.SelectMany(c => c.Value.SubTree()).Union(new[] { this });
        }

        public ComponentTreeNode Root { get; private set; }

        public ComponentTreeNode(TvComponentMetadata cmp, ComponentTreeNode parent = null)
        {
            Parent = parent;
            Root = parent?.Root ?? this;
            _childs = new Dictionary<Guid, ComponentTreeNode>();
            Data = cmp;
            Level = parent != null ? parent.Level + 1 : 0;
            _tags = new Dictionary<Type, object>();
            IsDismissed = false;
        }

        public TTag GetTag<TTag>()
        {
            return _tags.TryGetValue(typeof(TTag), out object value) ? (TTag)value : default;
        }

        public void SetTag<TTag>(TTag value)
        {
            var key = typeof(TTag);
            if (_tags.ContainsKey(key))
            {
                _tags[key] = value;
            }
            else
            {
                _tags.Add(key, value);
            }
        }

        public bool HasTag<TTag>()
        {
            return _tags.ContainsKey(typeof(TTag));
        }


        public ComponentTreeNode Add(TvComponentMetadata cdata)
        {
            var node = new ComponentTreeNode(cdata, this);
            _childs.Add(cdata.Id, node);
            return node;
        }

        public ComponentTreeNode Find(Guid id)
        {

            if (id == Data.Id) return this;
            return this.Descendants().FirstOrDefault(c => c.Data.Id == id);
        }

        public RemoveResult Remove(TvComponentMetadata toRemove)
        {
            if (toRemove == Data)
            {
                var deletedNodes = Delete();
                return new RemoveResult(RemoveStatus.RootDeleted, null, deletedNodes);
            }
            var child = Descendants().FirstOrDefault(c => c.Data == toRemove);
            if (child != null)
            {
                var nearParent = child.Parent;
                var deletedNodes = child.Delete();
                return new RemoveResult(RemoveStatus.ChildDeleted, nearParent, deletedNodes);
            }

            return new RemoveResult(RemoveStatus.NotFound, null, null);
        }

        private List<ComponentTreeNode> Delete()
        {
            var deleted = new List<ComponentTreeNode>();

            foreach (var child in _childs)
            {
                deleted.AddRange(child.Value.Delete());
            }
            if (Parent != null)
            {
                Parent._childs.Remove(Data.Id);
            }
            deleted.Add(this);
            return deleted;
        }

        public bool DismissChild(ComponentTreeNode child)
        {
            var key = child.Data.Id;
            if (_childs.ContainsKey(key))
            {
                _childs.Remove(child.Data.Id);
                child.Parent = null;
                child.Root = null;
                child.Level = 0;
                return true;
            }
            return false;
        }

        internal void Clear()
        {
            Parent = null;
            Root = null;
            _childs.Clear();
            _tags.Clear();
            Data = null;
            Level = 0;
            IsDismissed = true;
        }
    }

    public class ComponentTree : IComponentTree
    {

        class ComponentTreeItem
        {
            public ComponentTreeNode TreeNode { get; set; }
            public IEnumerable<ComponentTreeNode> FlattenedSubTree { get; set; }

            public ComponentTreeItem(ComponentTreeNode node)
            {
                TreeNode = node;
                FlattenedSubTree = Enumerable.Empty<ComponentTreeNode>();
            }
        }

        public class ComponentDependencyDescriptor
        {
            public TvComponentDependencyAttribute Attribute { get; }
            public PropertyInfo Property { get; }

            public ComponentDependencyDescriptor(TvComponentDependencyAttribute attr, PropertyInfo property)
            {
                Attribute = attr;
                Property = property;
            }
        }

        public class OwnedComponentDependencyDescriptor : ComponentDependencyDescriptor
        {
            public ITvBehavior Owner { get; }

            public OwnedComponentDependencyDescriptor(ITvBehavior behavior, TvComponentDependencyAttribute attr, PropertyInfo property)
                : base (attr, property)
            {
                Owner = behavior;
            }
        }


        private readonly LinkedList<ComponentTreeItem> _roots;

        private readonly Dictionary<string, AddComponentOptions> _pendingAdds;
        private readonly Dictionary<string, ComponentTreeNode> _pendingRemovalsPhase1;
        private readonly Dictionary<string, ComponentTreeNode> _pendingRemovalsPhase2;
        private readonly List<IViewport> _viewportsToClear;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITuiEngine _engine;
        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;
        public event EventHandler TreeUpdated;

        private List<OwnedComponentDependencyDescriptor> _pendingComponentDependencies;

        public IEnumerable<TvComponent> Components => NodesList.Select(node => node.Data.Component);

        public IEnumerable<ComponentTreeNode> NodesList
        {
            get
            {
                foreach (var roottree in _roots)
                {
                    foreach (var node in roottree.FlattenedSubTree) yield return node;
                }
            }
        }

        public TvComponent GetComponent(string name) => NodesList.FirstOrDefault(node => node.Data.Component.Name == name)?.Data.Component;

        public bool Remove(TvComponent component)
        {
            var name = component.Name;
            var node = FindNodeById(component.ComponentId);
            if (node != null)
            {
                
                node.Data.Status = TvComponentStatus.Removing;
                foreach (var child in node.SubTree())
                {
                    child.Data.Status = TvComponentStatus.Removing;
                }
                _pendingRemovalsPhase1.Add(name, node);
                return true;
            }

            return false;
        }

        private void DoPendingRemovalsPhase1()
        {
            if (_pendingRemovalsPhase1.Count == 0)
            {
                return;
            }
            var toDelete = _pendingRemovalsPhase1.ToArray();
            _pendingRemovalsPhase1.Clear();
            foreach (var kvp in toDelete)
            {
                var subtree = kvp.Value.SubTree();
                var canBeUnmounted = subtree.Any(c => c.Data.CanBeUnmountedFrom(_engine));
                if (canBeUnmounted)
                {
                    _pendingRemovalsPhase2.Add(kvp.Key, kvp.Value);
                    _viewportsToClear.AddRange(subtree.SelectMany(c => c.Data.Component.Viewports.Select(v => v.Value)));
                }
            }
        }

        private void DoPendingRemovalsPhase2()
        {
            if (_pendingRemovalsPhase2.Count == 0)
            {
                return;
            }
            var rootsAffected = new List<ComponentTreeNode>();
            foreach (var kvp in _pendingRemovalsPhase2)
            {
                var deleteResult = DeleteComponent(kvp.Value.Data);

                if (deleteResult.Status == ComponentTreeNode.RemoveStatus.ChildDeleted && !rootsAffected.Contains(kvp.Value.Root))
                {
                    rootsAffected.Add(kvp.Value.Root);
                }
                else if (deleteResult.Status == ComponentTreeNode.RemoveStatus.RootDeleted)
                {
                    DeleteRootNode(kvp.Value);
                }

                var subtree = deleteResult.AllDeletedNodes;
                foreach (var node in subtree)
                {
                    var cmp = node.Data;
                    cmp.UnmountedFrom(_engine, this);
                    OnComponentRemoved(cmp, node);
                    if (node.HasParent)
                    {
                        node.Parent.Data.RaiseChildUnmounted(cmp, node);
                    }
                }
                foreach (var node in subtree)
                {
                    node.Data.Status = TvComponentStatus.Dettached;
                    node.Clear();
                }

            }

            _pendingRemovalsPhase2.Clear();
            FlattenTree(rootsAffected);
            OnTreeUpdated();

        }

        private void DeleteRootNode(ComponentTreeNode value)
        {
            var item = _roots.First(r => r.TreeNode == value);
            _roots.Remove(item);
        }

        private ComponentTreeNode.RemoveResult DeleteComponent(TvComponentMetadata cdata)
        {
            foreach (var root in _roots)
            {
                var removeResult = root.TreeNode.Remove(cdata);

                if (removeResult.Status == ComponentTreeNode.RemoveStatus.ChildDeleted ||
                    removeResult.Status == ComponentTreeNode.RemoveStatus.RootDeleted)
                {
                    return removeResult;
                }
            }

            return ComponentTreeNode.RemoveResult.NotFound();
        }

        private void FlattenTree(IEnumerable<ComponentTreeNode> roots)
        {
            foreach (var root in roots)
            {
                var currentRoot = _roots.FirstOrDefault(r => r.TreeNode == root);
                currentRoot.FlattenedSubTree = root.SubTree();
            }
        }

        private ComponentTreeNode FindNodeById(Guid id)
        {
            foreach (var root in _roots)
            {
                var node = root.TreeNode.Find(id);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        public ComponentTree(ITuiEngine engine, IServiceProvider serviceProvider)
        {
            _roots = new LinkedList<ComponentTreeItem>();
            _pendingAdds = new Dictionary<string, AddComponentOptions>();
            _pendingRemovalsPhase1 = new Dictionary<string, ComponentTreeNode>();
            _pendingRemovalsPhase2 = new Dictionary<string, ComponentTreeNode>();
            _viewportsToClear = new List<IViewport>();
            _engine = engine;
            _serviceProvider = serviceProvider;
            _pendingComponentDependencies = new List<OwnedComponentDependencyDescriptor>();
        }

        public TvComponentMetadata AddAfter(TvComponent componentToAdd, TvComponent componentBefore)
        {
            _pendingAdds.Add(componentToAdd.Name, AddComponentOptions.AddAfter(componentToAdd.Metadata, componentBefore.Metadata));
            componentToAdd.Metadata.Status = TvComponentStatus.Adding;
            return componentToAdd.Metadata;
        }

        public TvComponentMetadata AddAsChild(TvComponent componentToAdd, TvComponent parent, Action<IAddChildComponentOptions> options = null)
        {
            var addOptions = AddComponentOptions.AddAsChild(componentToAdd.Metadata, parent.Metadata);
            options?.Invoke(addOptions);
            _pendingAdds.Add(componentToAdd.Name, addOptions);
            componentToAdd.Metadata.Status = TvComponentStatus.Adding;
            return componentToAdd.Metadata;
        }

        public TvComponentMetadata Add(TvComponent componentToAdd, Action<AddComponentOptions> addOptions = null)
        {
            var options = new AddComponentOptions(componentToAdd.Metadata);
            addOptions?.Invoke(options);
            _pendingAdds.Add(componentToAdd.Name, options);
            componentToAdd.Metadata.Status = TvComponentStatus.Adding;
            return componentToAdd.Metadata;
        }

        private void DoPendingAdds()
        {
            if (_pendingAdds.Count == 0)
            {
                return;
            }

            var toAdd = _pendingAdds.ToArray();

            var affectedRoots = new List<ComponentTreeNode>();
            var addedNodesOptions = new List<(AddComponentOptions Options, ComponentTreeNode Node)>();

            foreach (var kvp in toAdd)
            {
                var addOptions = kvp.Value;
                ComponentTreeNode nodeAdded = null;

                if (addOptions.ParentId != Guid.Empty)
                {
                    var parent = FindNodeById(addOptions.ParentId);
                    if (parent != null)
                    {
                        nodeAdded = parent.Add(addOptions.ComponentMetadata);
                    }
                }
                else if (addOptions.Before != null)
                {
                    var cnode = _roots.First;
                    while (cnode != null)
                    {
                        if (cnode.Value.TreeNode.Data == addOptions.Before)
                        {
                            nodeAdded = new ComponentTreeNode(addOptions.ComponentMetadata);
                            _roots.AddAfter(cnode, new ComponentTreeItem(nodeAdded));
                            break;
                        }
                        cnode = cnode.Next;
                    }
                    Debug.Assert(nodeAdded != null, "AddAfter: Before node not found!! (it is a root?");
                }
                else
                {
                    nodeAdded = new ComponentTreeNode(addOptions.ComponentMetadata);
                    _roots.AddLast(new ComponentTreeItem(nodeAdded));
                }

                if (nodeAdded != null)
                {
                    if (!affectedRoots.Contains(nodeAdded.Root))
                    {
                        affectedRoots.Add(nodeAdded.Root);
                    }
                    _pendingAdds.Remove(kvp.Key);
                    CreateNeededBehaviors(addOptions.ComponentMetadata.Component);
                    addedNodesOptions.Add((Options: addOptions, Node: nodeAdded));
                }
            }

            FlattenTree(affectedRoots);

            foreach (var optionsNodeAdded in addedNodesOptions)
            {
                var addOptions = optionsNodeAdded.Options;
                var nodeAdded = optionsNodeAdded.Node;
                TryResolveCurrentPendingDependencies(addOptions.ComponentMetadata.Component);
                TryResolveNewComponentAddedDependencies(addOptions.ComponentMetadata);
                addOptions.ComponentMetadata.MountedTo(_engine, this, nodeAdded, addOptions);
                addOptions.ComponentMetadata.Component.Invalidate(InvalidateReason.FullDrawRequired);
                OnComponentAdded(addOptions.ComponentMetadata, nodeAdded);
                if (nodeAdded.HasParent && addOptions.NotifyParentOnAdd)
                {
                    nodeAdded.Parent.Data.RaiseChildMounted(addOptions.ComponentMetadata, nodeAdded, addOptions);
                }
                addOptions.AfterAddAction?.Invoke(_engine);
                nodeAdded.Data.Status = TvComponentStatus.Running;
            }
            OnTreeUpdated();

            foreach (var optionsNodeAdded in addedNodesOptions)
            {
                optionsNodeAdded.Options.ComponentMetadata.RaiseTreeUpdatedByMount(this);
            }
        }

        private void TryResolveCurrentPendingDependencies(TvComponent newComponentAdded)
        {
            var pendings = _pendingComponentDependencies.Where(desc => desc.Attribute.Name == newComponentAdded.Name);
            if (!pendings.Any()) { return; }
            var toDelete = pendings.ToList();
            foreach (var desc in pendings) { ResolveDependency(desc, newComponentAdded); }
            foreach (var descriptorToDelete in toDelete)
            {
                _pendingComponentDependencies.Remove(descriptorToDelete);
            }
        }

        private void TryResolveNewComponentAddedDependencies(TvComponentMetadata metadata)
        {
            foreach (var behaviorMetadata in metadata.Component.BehaviorsMetadatas)
            {
                var type = behaviorMetadata.Behavior.GetType();
                var descriptors = type.GetProperties()
                    .Where(p => p.CanWrite)
                    .Select(p => new OwnedComponentDependencyDescriptor(behaviorMetadata.Behavior, p.GetCustomAttribute<TvComponentDependencyAttribute>(), p))
                    .Where(desc => desc.Attribute != null)
                    .Union(behaviorMetadata.Dependencies);

                foreach (var descriptor in descriptors)
                {
                    switch (descriptor.Attribute.Binding)
                    {
                        case DependencyBinding.IfAlreadyCreated:
                            TryResolveInmediateDependency(descriptor);
                            break;
                        case DependencyBinding.WhenCreate:
                            var resolved = TryResolveInmediateDependency(descriptor);
                            if (!resolved)
                            {
                                _pendingComponentDependencies.Add(descriptor);
                            }
                            break;
                    }
                }
            }
        }


        private void ResolveDependency(OwnedComponentDependencyDescriptor descriptor, TvComponent solver)
        {
            descriptor.Property.SetValue(descriptor.Owner, solver);
        }

        private bool TryResolveInmediateDependency(OwnedComponentDependencyDescriptor descriptor)
        {
            var componentFound = GetComponent(descriptor.Attribute.Name);
            descriptor.Property.SetValue(descriptor.Owner, componentFound);
            return componentFound != null;
        }

        private void CreateNeededBehaviors(TvComponent component)
        {
            var behaviorsToBeCreated = component.BehaviorsMetadatas.Where(bm => !bm.Created).ToList();
            foreach (var bm in behaviorsToBeCreated)
            {
                bm.CreateBehavior(_serviceProvider);
            }
        }

        private void OnTreeUpdated()
        {
            TreeUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void OnComponentAdded(TvComponentMetadata metadata, ComponentTreeNode node)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata, node));
        }

        private void OnComponentRemoved(TvComponentMetadata metadata, ComponentTreeNode node)
        {
            ComponentRemoved?.Invoke(this, new TreeUpdatedEventArgs(metadata, node));
        }

        public void ClearViewport(IViewport viewportToClear)
        {
            if (viewportToClear != null)
            {
                _viewportsToClear.Add(viewportToClear);
            }
        }

        public void Clear()
        {
            foreach (var child in Components)
            {
                Remove(child);
            }
        }

        internal void Update(ITvConsoleEvents evts)
        {
            DoPendingRemovalsPhase1();
            DoPendingAdds();
            foreach (var node in NodesList)
            {

                var component = node.Data.Component;
                var metadata = node.Data;
                if (metadata.Status == TvComponentStatus.Running)
                {
                    component.Update(new UpdateContext(evts, new ComponentLocator(this, node)));
                    if (metadata.PropagateStatusToChildren && component.NeedToRedraw != RedrawNeededAction.None)
                    {
                        var childs = metadata.TreeNode.Descendants();
                        foreach (var childnode in childs)
                        {
                            if (childnode.Data.AdmitStatusPropagation)
                            {
                                childnode.Data.Component.Invalidate(InvalidateReason.StateChanged);
                            }
                        }
                    }
                }
            }

        }

        internal void Draw(VirtualConsole console, bool force)
        {
            foreach (var viewport in _viewportsToClear)
            {
                console.Clear(viewport);
            }

            _viewportsToClear.Clear();

            foreach (var cdata in NodesList
                .Where(c => force || c.Data.Component.NeedToRedraw != RedrawNeededAction.None))
            {
                if (cdata.Data.Status == TvComponentStatus.Running)
                {
                    cdata.Data.Component.Draw(console, cdata);
                }
            }
            DoPendingRemovalsPhase2();
        }

    }
}
