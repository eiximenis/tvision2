using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls.Extensions;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    internal class ControlsTree : IControlsTree
    {

        private TvControlMetadata _focused;
        private TvControlMetadata _previousFocused;
        public event EventHandler<FocusChangedEventArgs> FocusChanged;
        public IEnumerable<TvControlMetadata> ControlsMetadata => _controls;
        private TvControlMetadata _controlWithFocusCaptured;
        private readonly IComponentTree _componentsTree;
        private readonly LinkedList<TvControlMetadata> _controls;
        private bool _needToMoveFocusWhenTreeUpdated;

        public ControlsTree(ITuiEngine engine)
        {
            _needToMoveFocusWhenTreeUpdated = false;
            _controls = new LinkedList<TvControlMetadata>();
            _componentsTree = engine.UI;
            _focused = null;
            _previousFocused = null;
            _controlWithFocusCaptured = null;
            _componentsTree.ComponentAdded += OnComponentAdded;
            _componentsTree.ComponentRemoved += OnComponentRemoved;
            _componentsTree.TreeUpdated += OnComponentsTreeUpdated;
        }

        private void OnComponentsTreeUpdated(object sender, EventArgs e)
        {
            _controls.Clear();
            var controls = _componentsTree.NodesList.Where(n => n.HasTag<TvControlMetadata>()).Select(n => n.GetTag<TvControlMetadata>())       ;
            foreach (var control in controls)
            {
                _controls.AddLast(control);
            }

            if (_focused == null)
            {
                TryToSetInitialFocus();
            }
            else if (_needToMoveFocusWhenTreeUpdated)
            {
                MoveFocusToNext();
                _needToMoveFocusWhenTreeUpdated = false;
            }


        }

        private void OnComponentAdded(object sender, TreeUpdatedEventArgs e)
        {
            var cdata = e.Node.GetTag<TvControlMetadata>();
            if (cdata == null)
            {
                return;
            }
            cdata.OwnerTree = this;
        }

        private void OnComponentRemoved(object sender, TreeUpdatedEventArgs e)
        {
            var cdata = e.Node.GetTag<TvControlMetadata>();
            if (cdata == null)
            {
                return;
            }
            cdata.OwnerTree = null;
            if (cdata == _focused)
            {
                _needToMoveFocusWhenTreeUpdated = true;
            }
        }

        public TvControlMetadata this[Guid id]
        {
            get
            {
                return _componentsTree.NodesList.FirstOrDefault(node => node.ControlMetadata()?.ControlId == id)?.ControlMetadata();
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

        public void CaptureFocus(TvControlMetadata control)
        {
            _controlWithFocusCaptured = control;
        }

        public void FreeFocus()
        {
            _controlWithFocusCaptured = null;
        }


        public bool MoveFocusToNext()
        {
            if (!_controls.Any())
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


        private void OnFocusChanged(TvControlMetadata previous)
        {
            var handler = FocusChanged;
            handler?.Invoke(this, new FocusChangedEventArgs(this, previous, _focused));
        }
    }
}
