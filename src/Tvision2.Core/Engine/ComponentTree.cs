using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public class ComponentTree
    {
        private readonly Dictionary<string, TvComponentMetadata> _components;

        public ComponentTree()
        {
            _components = new Dictionary<string, TvComponentMetadata>();
        }

        public void Add(TvComponent component, int zIndex = 0)
        {
            if (zIndex != 0)
            {
                component.Style.ZIndex = zIndex;
            }
            var viewport = new Viewport(component.Style);
            _components.Add(component.Name, new TvComponentMetadata(component, viewport));
        }

        public void Focus(TvComponent componentToFocus) => Focus(componentToFocus, unfocusOthers: true);
        public void Focus(TvComponent componentToFocus, bool unfocusOthers)
        {
            TvComponentMetadata metadata = null;
            if (unfocusOthers)
            {
                foreach (var cdata in _components.Values)
                {
                    cdata.Focused = false;
                    if (metadata == null && cdata.Component == componentToFocus)
                    {
                        metadata = cdata;
                        metadata.Focused = true;
                    }
                }
            }
        }

        internal void Update(TvConsoleEvents evts)
        {
            foreach (var cdata in _components.Values)
            {
                cdata.Component.Update(evts);
            }
        }

        internal void Draw(VirtualConsole console, bool force)
        {
            foreach (var cdata in _components.Values
                .Where(c => force || c.Component.IsDirty))
            {
                cdata.Component.Draw(cdata.Viewport, console);
            }
        }


    }
}
