using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Controls.Extensions;
using Tvision2.Core;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    class ControlsTree : IControlsTree, ICustomItemsProviderClient
    {
        private readonly LinkedList<TvControlMetadata> _controls;

        private TvControlMetadata _focused;
        private IComponentTree _componentTree;

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

        public void Add(TvControlMetadata cdata)
        {
            _controls.AddLast(cdata);
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
                    _focused.Control.Data.Style.RemoveClass("focused");
                    _componentTree.RemoveFromRespondersChain(_focused.ComponentMetadata);
                }
                next.Control.Data.Style.AddClass("focused");
                _componentTree.AddToResponderChain(next.ComponentMetadata);
                _focused = next;
            }

        }

        void ICustomItemsProviderClient.ReceiveProvider(ICustomItemsProvider provider)
        {
            _componentTree = provider.GetCustomItem<IComponentTree>();
        }
    }
}
