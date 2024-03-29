﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Tvision2.Controls.Extensions;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    internal class ControlsTree : IControlsTree
    {

        private TvControlMetadata? _focused;
        private TvControlMetadata? _previousFocused;
        public event EventHandler<FocusChangedEventArgs> FocusChanged;
        public IEnumerable<TvControlMetadata> ControlsMetadata => _controls;
        private TvControlMetadata? _controlWithFocusCaptured;
        private readonly IComponentTree _componentsTree;
        private readonly LinkedList<TvControlMetadata> _controls;
        private bool _needToMoveFocusWhenTreeUpdated;
        private int _nextTabGroup;

        private LinkedList<TvControlMetadata> _responders;

        public ControlsTree(ITuiEngine engine)
        {
            _nextTabGroup = -1;
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

            var ctls = _componentsTree.NodesList.Where(n => (n.Data.Status == TvComponentStatus.Adding || n.Data.Status == TvComponentStatus.Running) && 
                n.HasTag<TvControlMetadata>()).Select(n => n.GetTag<TvControlMetadata>()).GroupBy(m => m.TabGroup).OrderBy(m => m.Key);

            var controls = ctls.SelectMany(c => c.OrderBy(ci => ci.TabLevel).ThenBy(ci => ci.TabOrder));

            
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
                ReleaseFocus();
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

        public TvControlMetadata LocateControlAt(TvPoint pos)
        {

            var layer = Layer.Bottom;
            TvControlMetadata selected = null;

            foreach (var cmdata in _controls)
            {
                var ccomp = cmdata.Control.AsComponent();
                var vp = ccomp.Viewport;
                if (vp.ContainsPoint(pos))
                {
                    if (vp.ZIndex >= layer)
                    {
                        selected = cmdata;
                        layer = vp.ZIndex;
                    }
                }
            }

            return selected;
        }



        // Returns the next control candidate to have focus. If no more controls, returns the 1st control again
        // Note that if responders list has only one control, NextControlForFocus will return always this single control
        public TvControlMetadata NextControlForFocus(TvControlMetadata? current)
        {
            var next = current != null ? _responders.Find(current)?.Next : null;
            return next != null ? next.Value : _responders.First.Value; 
        }

        public TvControlMetadata PreviousControlForFocus(TvControlMetadata current)
        {
            return _responders.Find(current)?.Previous?.Value;
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

        public TvControlMetadata? First() => _controls.First?.Value;

        public void CaptureFocus(TvControlMetadata control)
        {
            _controlWithFocusCaptured = control;
            _responders = new LinkedList<TvControlMetadata>();
            var ctls = control.ComponentNode.SubTree().Where(n => n.HasTag<TvControlMetadata>()).Select(n => n.GetTag<TvControlMetadata>());
            foreach (var ctl in ctls)
            {
                _responders.AddLast(ctl);
            }
        }

        public void ReleaseFocus()
        {
            _controlWithFocusCaptured = null;
            _responders = _controls;
        }


        public bool MoveFocusToNext()
        {
            if (!_responders.Any())
            {
                return false;
            }
            var currentFocused = _focused;

            var next = NextControlForFocus(currentFocused);
            var prev = next;

            while (!IsValidTargetForFocus(currentFocused, next) && next != currentFocused)
            {
                next = NextControlForFocus(next);
                if (next == prev)       // Control to focus is itself, who is already focused. Nothing to do.
                {
                    return false;              
                }
                prev = next;
            }

            return Focus(next);
        }

        internal int GetNextTabGroup() => ++_nextTabGroup;

        // Returns if the control is a valid target for focus according its metadata.
        private bool IsValidTargetForFocus(TvControlMetadata currentFocused, TvControlMetadata nextWanted)
        {
            return nextWanted.AcceptFocus(currentFocused);
        }

        private void TryToSetInitialFocus()
        {
            var toBeFocused = _controls.FirstOrDefault(x => x.AcceptFocus(null));
            TransferFocus(null, toBeFocused);
        }

        private bool TransferFocus(TvControlMetadata? toBeUnfocused, TvControlMetadata? toBeFocused)
        {
            if (toBeUnfocused == toBeFocused)
            {
                return false;
            }
            if (toBeFocused != null)
            {
                _previousFocused = _focused;
                _focused = toBeFocused;
                toBeUnfocused?.Unfocus();
                toBeFocused.DoFocus();
                OnFocusChanged(toBeUnfocused!);
            }
            return true;
        }


        private void OnFocusChanged(TvControlMetadata previous)
        {
            var handler = FocusChanged;
            handler?.Invoke(this, new FocusChangedEventArgs(this, previous, _focused));
        }
    }
}
