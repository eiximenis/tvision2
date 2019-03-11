using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls.Extensions;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    internal class ControlsTree : IControlsTree
    {
        private readonly LinkedList<TvControlMetadata> _controls;
        private Dictionary<Guid, TvControlMetadata> _indexedControls;
        private IComponentTree _componentTree;
        private TvControlMetadata _focused;
        private TvControlMetadata _previousFocused;
        public event EventHandler<FocusChangedEventArgs> FocusChanged;
        public IEnumerable<TvControlMetadata> ControlsMetadata => _controls;

        public ControlsTree()
        {
            _controls = new LinkedList<TvControlMetadata>();
            _focused = null;
            _previousFocused = null;
            _componentTree = null;
            _indexedControls = new Dictionary<Guid, TvControlMetadata>();
        }

        public void InsertAfter(TvControlMetadata cdata, int position)
        {
            if (position < 0)
            {
                throw new ArgumentException("position must be >=0", nameof(position));
            }

            var current = _controls.NodeAt(position);
            _controls.AddAfter(current, cdata);
            _indexedControls.Add(cdata.ControlId, cdata);
        }

        internal void AttachTo(IComponentTree comtree)
        {
            _componentTree = comtree;
        }

        public void Add(TvControlMetadata cdata)
        {
            if (cdata.OwnerTree != null && cdata.OwnerTree != this)
            {
                throw new InvalidOperationException("TvControl belongs to another ControlsTree. Operation not allowed.");
            }
            _controls.AddLast(cdata);
            _indexedControls.Add(cdata.ControlId, cdata);
            cdata.OwnerTree = this;

            if (_focused == null)
            {
                TryToSetInitialFocus();
            }
        }

        public void Remove(TvControlMetadata cdata)
        {
            if (_indexedControls.ContainsKey(cdata.ControlId))
            {
                _controls.Remove(cdata);
                _indexedControls.Remove(cdata.ControlId);
                cdata.OwnerTree = null;
                if (_focused == cdata)
                {
                    MoveFocusToNext();
                }
            }
        }


        public TvControlMetadata NextControl(TvControlMetadata current)
        {
            var next = current != null ? _controls.Find(current)?.Next : null;
            return next != null ? next.Value : _controls.First.Value;
        }

        public TvControlMetadata PreviousControl(TvControlMetadata current)
        {
            return _controls.Find(current)?.Previous?.Value;
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

        public TvControlMetadata First() => _controls.First?.Value;

        public bool MoveFocusToNext()
        {
            if (!_controls.Any())
            {
                return false;
            }
            var currentFocused = _focused;
            var next = NextControl(_focused);
            while (!IsValidTargetForFocus(currentFocused, next) && next != currentFocused )
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
            if (currentFocused != null && currentFocused.OwnerId == nextWanted.ControlId) return false;
            return nextWanted.AcceptFocus(currentFocused);
        }
        private void TryToSetInitialFocus()
        {
            var toBeFocused = _controls.FirstOrDefault(x => x.AcceptFocus(null));
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

        public TvControlMetadata this[Guid id] => 
            _indexedControls.TryGetValue(id, out TvControlMetadata result) ? result : null;

        private void OnFocusChanged(TvControlMetadata previous)
        {
            var handler = FocusChanged;
            handler?.Invoke(this, new FocusChangedEventArgs(this, previous, _focused));
        }
    }
}
