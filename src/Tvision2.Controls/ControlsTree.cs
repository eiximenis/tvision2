using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls.Extensions;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    internal class ControlTreeNode
    {

        public struct RemoveResult
        {
            private readonly List<ControlTreeNode> _allDeletedNodes;
            public RemoveStatus Status { get; }
            public ControlTreeNode NearestParent { get; }

            public IEnumerable<ControlTreeNode> AllDeletedNodes { get { return _allDeletedNodes ?? Enumerable.Empty<ControlTreeNode>(); } }

            public RemoveResult(RemoveStatus status, ControlTreeNode nearestParent, List<ControlTreeNode> deletedNodes)
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

        public ControlTreeNode Parent { get; private set; }
        private Dictionary<Guid, ControlTreeNode> _childs;

        public TvControlMetadata Control { get; }

        public IEnumerable<ControlTreeNode> Childs
        {
            get => _childs.Values;
        }


        public IEnumerable<ControlTreeNode> Descendants()
        {
            return _childs.SelectMany(c => c.Value.SubTree());
        }
        public IEnumerable<ControlTreeNode> SubTree()
        {
            return _childs.SelectMany(c => c.Value.SubTree()).Union(new[] { this });
        }

        public ControlTreeNode(TvControlMetadata control, ControlTreeNode parent = null)
        {
            Parent = parent;
            _childs = new Dictionary<Guid, ControlTreeNode>();
            Control = control;
        }

        public void Add(TvControlMetadata cdata)
        {
            var node = new ControlTreeNode(cdata, this);
            _childs.Add(cdata.ControlId, node);
        }

        public ControlTreeNode Find(Guid id)
        {

            if (id == Control.ControlId) return this;
            return this.Descendants().FirstOrDefault(c => c.Control.ControlId == id);
        }

        public RemoveResult Remove(TvControlMetadata toRemove)
        {

            if (toRemove == Control)
            {
                var deletedNodes = Delete();
                return new RemoveResult(RemoveStatus.RootDeleted, null, deletedNodes);
            }
            var child = Descendants().FirstOrDefault(c => c.Control == toRemove);
            if (child != null)
            {
                var nearParent = child.Parent;
                var deletedNodes = child.Delete();
                return new RemoveResult(RemoveStatus.ChildDeleted, nearParent, deletedNodes);
            }

            return new RemoveResult(RemoveStatus.NotFound, null, null);
        }

        private List<ControlTreeNode> Delete()
        {
            var deleted = new List<ControlTreeNode>();

            foreach (var child in _childs)
            {
                deleted.AddRange(child.Value.Delete());
            }
            if (Parent != null)
            {
                Parent._childs.Remove(Control.ControlId);
            }
            Control.OwnerTree = null;
            deleted.Add(this);
            return deleted;
        }

        public bool DismissChild(ControlTreeNode child)
        {
            var key = child.Control.ControlId;
            if (_childs.ContainsKey(key)) {
                _childs.Remove(child.Control.ControlId);
                child.Parent = null;
                return true;
            }
            return false;
        }

        public void Adopt(ControlTreeNode nodeToAdopt)
        {
            nodeToAdopt.Parent = this;
            _childs.Add(nodeToAdopt.Control.ControlId, nodeToAdopt);
        }
    }

    internal class ControlsTree : IControlsTree
    {

        private readonly List<ControlTreeNode> _roots;
        private readonly LinkedList<TvControlMetadata> _flattenedTree;
        private TvControlMetadata _focused;
        private TvControlMetadata _previousFocused;
        public event EventHandler<FocusChangedEventArgs> FocusChanged;
        public IEnumerable<TvControlMetadata> ControlsMetadata => _flattenedTree;

        private TvControlMetadata _controlWithFocusCaptured;

        public ControlsTree()
        {
            _roots = new List<ControlTreeNode>();
            _flattenedTree = new LinkedList<TvControlMetadata>();
            _focused = null;
            _previousFocused = null;
            _controlWithFocusCaptured = null;
        }

        public TvControlMetadata this[Guid id]
        {
            get
            {
                return _flattenedTree.FirstOrDefault(c => c.ControlId == id);
            }
        }

        public void Add(TvControlMetadata cdata)
        {
            if (cdata.OwnerTree != null && cdata.OwnerTree != this)
            {
                throw new InvalidOperationException("TvControl belongs to another ControlsTree. Operation not allowed.");
            }
            cdata.OwnerTree = this;
            _roots.Add(new ControlTreeNode(cdata));
            if (_focused == null)
            {
                TryToSetInitialFocus();
            }

            FlattenTree(_controlWithFocusCaptured);
        }

        private void FlattenTree(TvControlMetadata root = null)
        {
            _flattenedTree.Clear();

            IEnumerable<TvControlMetadata> flattened = null;
            if (root == null)
            {
                flattened = _roots.SelectMany(r => r.SubTree().Select(n => n.Control));
            }
            else
            {
                var rootNode = FindNodeById(root.ControlId);
                flattened = rootNode.SubTree().Select(n => n.Control);
            }

            foreach (var item in flattened)
            {
                _flattenedTree.AddLast(item);
            }
        }

        public IEnumerable<TvControlMetadata> Childs(TvControlMetadata cdata)
        {
            var node = FindNodeById(cdata.ControlId);
            if (node != null)
            {
                return node.Childs.Select(c => c.Control);
            }
            return Enumerable.Empty<TvControlMetadata>();
        }
        public IEnumerable<TvControlMetadata> Descendants(TvControlMetadata cdata)
        {
            var node = FindNodeById(cdata.ControlId);
            if (node != null)
            {
                return node.Descendants().Select(c => c.Control);
            }
            return Enumerable.Empty<TvControlMetadata>();

        }

        public void Adopt(TvControlMetadata newParent, TvControlMetadata toAdopt)
        {
            var nodeToAdopt = FindNodeById(toAdopt.ControlId);
            var nodeParent = FindNodeById(newParent.ControlId);
            var oldParent = nodeToAdopt.Parent;
            var dismissed = oldParent.DismissChild(nodeToAdopt);
            // TODO: Assert dismissed is TRUE
            nodeParent.Adopt(nodeToAdopt);
            FlattenTree(_controlWithFocusCaptured);
        }


        private ControlTreeNode FindNodeById(Guid id)
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

        public bool AddAsChild(TvControlMetadata cdata, Guid parentId)
        {
            var node = FindNodeById(parentId);
            if (node != null)
            {
                node.Add(cdata);
                FlattenTree(_controlWithFocusCaptured);
            }
            cdata.OwnerTree = this;
            return node != null;
        }

        public void Remove(TvControlMetadata cdata)
        {
            foreach (var root in _roots)
            {
                var removeResult = root.Remove(cdata);

                if (removeResult.Status == ControlTreeNode.RemoveStatus.ChildDeleted ||
                    removeResult.Status == ControlTreeNode.RemoveStatus.RootDeleted)
                {
                    FlattenTree(_controlWithFocusCaptured);
                    var allDeletedNodes = removeResult.AllDeletedNodes;
                    if (allDeletedNodes.Any(x => x.Control == _focused))
                    {
                        MoveFocusToNext();
                    }
                    cdata.OwnerTree = null;
                    break;
                }
            }
        }


        public TvControlMetadata NextControl(TvControlMetadata current)
        {
            var next = current != null ? _flattenedTree.Find(current)?.Next : null;
            return next != null ? next.Value : _flattenedTree.First.Value;
        }

        public TvControlMetadata PreviousControl(TvControlMetadata current)
        {
            return _flattenedTree.Find(current)?.Previous?.Value;
        }

        public TvControlMetadata CurrentFocused() => _focused;

        public bool ReturnFocusToPrevious()
        {
            return TransferFocus(_focused, _previousFocused);
        }

        public bool Focus(TvControlMetadata controlToFocus)
        {
            return TransferFocus(_focused, controlToFocus);
        }

        public TvControlMetadata First() => _flattenedTree.First?.Value;

        public void CaptureFocus(TvControlMetadata control)
        {
            _controlWithFocusCaptured = control;
            FlattenTree(_controlWithFocusCaptured);
        }

        public void FreeFocus()
        {
            _controlWithFocusCaptured = null;
            FlattenTree();
        }


        public bool MoveFocusToNext()
        {
            if (!_flattenedTree.Any())
            {
                return false;
            }
            var currentFocused = _focused;
            var next = NextControl(_focused);
            while (!IsValidTargetForFocus(currentFocused, next) && next != currentFocused)
            {
                next = NextControl(next);
            }
            if (next.AcceptFocus(currentFocused) == false)
            {
                return false;
            }
            return Focus(next);
        }



        private bool IsValidTargetForFocus(TvControlMetadata currentFocused, TvControlMetadata nextWanted)
        {
            if (currentFocused != null && currentFocused.ParentId == nextWanted.ControlId) return false;
            return nextWanted.AcceptFocus(currentFocused);
        }
        private void TryToSetInitialFocus()
        {
            var toBeFocused = _flattenedTree.FirstOrDefault(x => x.AcceptFocus(null));
            TransferFocus(null, toBeFocused);
        }

        private bool TransferFocus(TvControlMetadata toBeUnfocused, TvControlMetadata toBeFocused)
        {
            if (toBeUnfocused == toBeFocused)
            {
                return false;
            }
            if (toBeFocused != null)
            {
                toBeUnfocused?.Unfocus();
                toBeFocused.DoFocus();
                OnFocusChanged(toBeUnfocused);
            }
            _previousFocused = _focused;
            _focused = toBeFocused;
            return true;
        }


        private void OnFocusChanged(TvControlMetadata previous)
        {
            var handler = FocusChanged;
            handler?.Invoke(this, new FocusChangedEventArgs(this, previous, _focused));
        }
    }
}
