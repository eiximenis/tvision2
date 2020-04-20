using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{

    internal class ComponentTreeNode
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
        }

        public enum RemoveStatus
        {
            NotFound = 0,
            RootDeleted = 1,
            ChildDeleted = 2
        }

        public ComponentTreeNode Parent { get; private set; }
        private Dictionary<Guid, ComponentTreeNode> _childs;

        public TvComponentMetadata Data { get; }

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

        public ComponentTreeNode(TvComponentMetadata cmp, ComponentTreeNode parent = null)
        {
            Parent = parent;
            _childs = new Dictionary<Guid, ComponentTreeNode>();
            Data = cmp;
        }

        public void Add(TvComponentMetadata cdata)
        {
            var node = new ComponentTreeNode(cdata, this);
            _childs.Add(cdata.Id, node);
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
                return true;
            }
            return false;
        }

        public void Adopt(ComponentTreeNode nodeToAdopt)
        {
            nodeToAdopt.Parent = this;
            _childs.Add(nodeToAdopt.Data.Id, nodeToAdopt);
        }
    }

    public class ComponentTree : IComponentTree
    {

        struct PendingAddData
        {
            public TvComponentMetadata Metadata { get; }
            public TvComponentMetadata Parent { get; }
            public TvComponentMetadata Before { get; }
            public Action<ITuiEngine> AfterAdd { get; }

            public PendingAddData(TvComponentMetadata metadata, TvComponentMetadata before, TvComponentMetadata parent, Action<ITuiEngine> afterAdd)
            {
                Metadata = metadata;
                Before = before;
                Parent = parent;
                AfterAdd = afterAdd;
            }

            public static PendingAddData AddLast(TvComponentMetadata metadata, Action<ITuiEngine> afterAdd) =>
                new PendingAddData(metadata, null, null, afterAdd);
            public static PendingAddData AddAfter(TvComponentMetadata metadata, TvComponentMetadata before, Action<ITuiEngine> afterAdd) =>
                new PendingAddData(metadata, before, null, afterAdd);
            public static PendingAddData AddAsChild(TvComponentMetadata metadata, TvComponentMetadata parent, Action<ITuiEngine> afterAdd) =>
                new PendingAddData(metadata, null, parent, afterAdd);
        }


        private readonly LinkedList<ComponentTreeNode> _roots;
        private readonly Dictionary<string, PendingAddData> _pendingAdds;
        private readonly Dictionary<string, TvComponentMetadata> _pendingRemovalsPhase1;
        private readonly Dictionary<string, TvComponentMetadata> _pendingRemovalsPhase2;
        private readonly List<IViewport> _viewportsToClear;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITuiEngine _engine;

        private readonly List<TvComponentMetadata> _flattened;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public IEnumerable<TvComponent> Components => _flattened.Select(cm => cm.Component);

        public TvComponent GetComponent(string name) => _flattened.FirstOrDefault(c => c.Component.Name == name)?.Component;

        public T GetInstanceOf<T>() => (T)_engine.ServiceProvider.GetService(typeof(T));


        public bool Remove(TvComponent component)
        {
            var name = component.Name;
            var node = FindNodeById(component.ComponentId);
            if (node != null)
            {
                _pendingRemovalsPhase1.Add(name, component.Metadata as TvComponentMetadata);
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
                var subtree = GetFlattenedTree(kvp.Value);
                var canBeUnmounted = subtree.Any(c => c.CanBeUnmountedFrom(_engine));
                if (canBeUnmounted)
                {
                    _pendingRemovalsPhase2.Add(kvp.Key, kvp.Value);
                    _viewportsToClear.AddRange(subtree.SelectMany(c => c.Component.Viewports.Select(v => v.Value)));
                }
            }
        }




        private void DoPendingRemovalsPhase2()
        {
            if (_pendingRemovalsPhase2.Count == 0)
            {
                return;
            }
            foreach (var kvp in _pendingRemovalsPhase2)
            {
                DeleteComponent(kvp.Value);
                var subtree = GetFlattenedTree(kvp.Value);
                foreach (var cmp in subtree)
                {
                    cmp.UnmountedFrom(_engine);
                    OnComponentRemoved(cmp);
                }
            }

            _pendingRemovalsPhase2.Clear();
            FlattenTree();

        }

        private bool DeleteComponent(TvComponentMetadata cdata)
        {
            foreach (var root in _roots)
            {
                var removeResult = root.Remove(cdata);

                if (removeResult.Status == ComponentTreeNode.RemoveStatus.ChildDeleted ||
                    removeResult.Status == ComponentTreeNode.RemoveStatus.RootDeleted)
                {
                    return true;
                }
            }

            return false;
        }

        private void FlattenTree()
        {
            _flattened.Clear();
            foreach (var item in GetFlattenedTree(null))
            {
                _flattened.Add(item);
            }

        }

        private IEnumerable<TvComponentMetadata> GetFlattenedTree(TvComponentMetadata root)
        {

            IEnumerable<TvComponentMetadata> flattened = null;
            if (root == null)
            {
                flattened = _roots.SelectMany(r => r.SubTree().Select(n => n.Data));
            }
            else
            {
                var rootNode = FindNodeById(root.Id);
                flattened = rootNode.SubTree().Select(n => n.Data);
            }

            return flattened;
        }


        private ComponentTreeNode FindNodeById(Guid id)
        {
            foreach (var root in _roots)
            {
                var node = root.Find(id);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        public ComponentTree(ITuiEngine engine, IServiceProvider serviceProvider)
        {
            _roots = new LinkedList<ComponentTreeNode>();
            _flattened = new List<TvComponentMetadata>();
            _pendingAdds = new Dictionary<string, PendingAddData>();
            _pendingRemovalsPhase1 = new Dictionary<string, TvComponentMetadata>();
            _pendingRemovalsPhase2 = new Dictionary<string, TvComponentMetadata>();
            _viewportsToClear = new List<IViewport>();
            _engine = engine;
            _serviceProvider = serviceProvider;
        }

        public IComponentMetadata Add(TvComponent component, Action<ITuiEngine> afterAddAction = null)
        {
            _pendingAdds.Add(component.Name, PendingAddData.AddLast(component.Metadata as TvComponentMetadata, afterAddAction));
            return component.Metadata;
        }

        public IComponentMetadata AddAfter(TvComponent componentToAdd, TvComponent componentBefore, Action<ITuiEngine> afterAddAction = null)
        {
            _pendingAdds.Add(componentToAdd.Name, PendingAddData.AddAfter(componentToAdd.Metadata as TvComponentMetadata, componentBefore.Metadata as TvComponentMetadata, afterAddAction));
            return componentToAdd.Metadata;
        }

        public IComponentMetadata AddAsChild(TvComponent componentToAdd, TvComponent parent, Action<ITuiEngine> afterAddAction = null)
        {
            _pendingAdds.Add(componentToAdd.Name, PendingAddData.AddAsChild(componentToAdd.Metadata as TvComponentMetadata, parent.Metadata as TvComponentMetadata, afterAddAction));
            return componentToAdd.Metadata;
        }


        private void DoPendingAdds()
        {
            if (_pendingAdds.Count == 0)
            {
                return;
            }

            var toAdd = _pendingAdds.ToArray();
            _pendingAdds.Clear();

            foreach (var kvp in toAdd)
            {
                var pendingAddData = kvp.Value;

                if (pendingAddData.Parent != null)
                {
                    var parent = FindNodeById(pendingAddData.Parent.Id);
                    Debug.Assert(parent != null);
                    parent.Add(pendingAddData.Metadata);
                }
                else if (pendingAddData.Before != null)
                {
                    var cnode = _roots.First;
                    while (cnode != null)
                    {
                        if (cnode.Value.Data == pendingAddData.Before)
                        {
                            _roots.AddAfter(cnode, new ComponentTreeNode(pendingAddData.Metadata));            // TODO: Parent add is here!!!
                            break;
                        }
                        cnode = cnode.Next;
                    }
                }
                else
                {
                    _roots.AddLast(new ComponentTreeNode(pendingAddData.Metadata));
                }

                CreateNeededBehaviors(pendingAddData.Metadata.Component);
                pendingAddData.Metadata.MountedTo(_engine);
                pendingAddData.Metadata.Component.Invalidate();
                pendingAddData.AfterAdd?.Invoke(_engine);
                OnComponentAdded(pendingAddData.Metadata);
            }

            FlattenTree();
        }

        private void CreateNeededBehaviors(TvComponent component)
        {
            var behaviorsToBeCreated = component.BehaviorsMetadatas.Where(bm => !bm.Created).ToList();
            foreach (var bm in behaviorsToBeCreated)
            {
                bm.CreateBehavior(_serviceProvider);
            }
        }

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private void OnComponentRemoved(IComponentMetadata metadata)
        {
            ComponentRemoved?.Invoke(this, new TreeUpdatedEventArgs(metadata));
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
            foreach (var cdata in _flattened)
            {
                cdata.Component.Update(evts);
            }

        }

        internal void Draw(VirtualConsole console, bool force)
        {
            foreach (var viewport in _viewportsToClear)
            {
                console.Clear(viewport);
            }

            foreach (var cdata in _flattened
                .Where(c => force || c.Component.NeedToRedraw != RedrawNeededAction.None))
            {
                cdata.Component.Draw(console);
            }
            _viewportsToClear.Clear();
            DoPendingRemovalsPhase2();
        }

    }
}
