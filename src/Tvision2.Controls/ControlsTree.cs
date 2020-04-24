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

        private LinkedList<TvControlMetadata> _responders;

        public ControlsTree(ITuiEngine engine)
        {
            _needToMoveFocusWhenTreeUpdated = false;
            _controls = new LinkedList<TvControlMetadata>();
            _responders = _controls;
            _componentsTree = engine.UI;
            _focused = null;
            _previousFocused = null;
            _controlWithFocusCaptured = null;
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

            if (_controlWithFocusCaptured != null)
            {
                CaptureFocus(_controlWithFocusCaptured);
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


        private void OnComponentRemoved(object sender, TreeUpdatedEventArgs e)
        {
            var cdata = e.Node.GetTag<TvControlMetadata>();
            if (cdata == null)
            {
                return;
            }
            if (_controlWithFocusCaptured == cdata)
            {
                FreeFocus();
            }
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



        public TvControlMetadata NextControlForFocus(TvControlMetadata current)
        {
            var next = current != null ? _responders.Find(current)?.Next : null;
            return next != null ? next.Value : _responders.First.Value;
        }

        public TvControlMetadata PreviousControlForFocus(TvControlMetadata current)
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
            _responders = new LinkedList<TvControlMetadata>();
            var ctls =  control.ComponentNode.SubTree().Where(n => n.HasTag<TvControlMetadata>()).Select(n => n.GetTag<TvControlMetadata>());
            foreach (var ctl in ctls)
            {
                _responders.AddLast(ctl);
            }
        }

        public void FreeFocus()
        {
            _controlWithFocusCaptured = null;
            _responders = _controls;
        }


        public bool MoveFocusToNext()
        {
            if (!_controls.Any())
            {
                return false;
            }
            var currentFocused = _focused;
            var next = NextControlForFocus(_focused);
            while (!IsValidTargetForFocus(currentFocused, next) && next != currentFocused)
            {
                next = NextControlForFocus(next);
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
