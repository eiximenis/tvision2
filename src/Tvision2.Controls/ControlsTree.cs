using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    class ControlsTree : IControlsTree
    {
        private readonly LinkedList<TvControlMetadata> _controls;
        private IComponentTree _componentTree;
        private TvControlMetadata _focused;

        public ControlsTree()
        {
            _controls = new LinkedList<TvControlMetadata>();
            _focused = null;
            _componentTree = null;
        }

        public void InsertAfter(TvControlMetadata cdata, int position)
        {
            if (position < 0)
            {
                throw new ArgumentException("position must be >=0", nameof(position));
            }

            var current = _controls.NodeAt(position);
            _controls.AddAfter(current, cdata);
        }


        public void Add(TvControlMetadata cdata) => _controls.AddLast(cdata);

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

        public void Focus(TvControlMetadata controlToFocus)
        {
            _focused = controlToFocus;
        }

        public TvControlMetadata First() => _controls.First?.Value;

        public void MoveFocusToNext()
        {
            var next = _focused != null ? NextControl(_focused) : _controls.First?.Value;
            if (next != null)
            {
                if (_focused != null)
                {
                    _focused.Control.Style.RemoveClass("focused");
                    _componentTree.RemoveFromRespondersChain(_focused.ComponentMetadata);
                }
                next.Control.Style.AddClass("focused");
                _componentTree.AddToResponderChain(next.ComponentMetadata);
                _focused = next;
            }

        }


        internal void AttachToComponentTree(IComponentTree componentTree)
        {
            _componentTree = componentTree;
            _componentTree.ComponentAdded += ComponenTree_ComponentAdded;
        }

        private void ComponenTree_ComponentAdded(object sender, TreeUpdatedEventArgs e)
        {
            var metadata = e.ComponentMetadata;
            var control = metadata.Component as ITvControl;
            if (control != null)
            {
                var cdata = new TvControlMetadata(metadata, control);
                _controls.AddLast(cdata);
            }
        }
    }
}
