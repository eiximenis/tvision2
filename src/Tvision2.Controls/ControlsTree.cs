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

        public IEnumerable<TvControlMetadata> ControlsMetadata => _controls;


        public ControlsTree()
        {
            _controls = new LinkedList<TvControlMetadata>();
            _focused = null;
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
            _controls.AddLast(cdata);
            _indexedControls.Add(cdata.ControlId, cdata);
        }

        public void Remove(TvControlMetadata cdata)
        {
            _controls.Remove(cdata);
            _indexedControls.Remove(cdata.ControlId);
            if (_focused == cdata)
            {
                _focused = null;
            }
        }


        public TvControlMetadata NextControl(TvControlMetadata current)
        {
            var next = _controls.Find(current)?.Next;
            return next != null ? next.Value : _controls.First.Value;
        }

        public TvControlMetadata PreviousControl(TvControlMetadata current)
        {
            return _controls.Find(current)?.Previous?.Value;
        }

        public TvControlMetadata CurrentFocused() => _focused;

        public bool Focus(TvControlMetadata controlToFocus)
        {
            if (_focused != controlToFocus)
            {
                if (_focused != null)
                {
                    _focused.Unfocus();
                }
                controlToFocus.Focus();
                _focused = controlToFocus;
                return true;
            }

            return false;
        }

        public TvControlMetadata First() => _controls.First?.Value;

        public bool MoveFocusToNext()
        {
            if (!_controls.Any())
            {
                return false;
            }

            var currentFocused = _focused ?? _controls.First.Value;

            var next = NextControl(_focused);
            while (!next.CanFocus && next != currentFocused)
            {
                next = NextControl(next);
            }
            return Focus(next);
        }

        public TvControlMetadata this[Guid id] => 
            _indexedControls.TryGetValue(id, out TvControlMetadata result) ? result : null;
    }
}
