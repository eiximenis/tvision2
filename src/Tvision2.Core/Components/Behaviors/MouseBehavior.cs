using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public class MouseBehaviorOptions
    {
        public MouseBehaviorOptions()
        {
            DispatchClicks = true;
            DispatchDoubleClicks = true;
            DispatchMouseDown = false;
            DispatchMouseUp = false;
        }

        public bool DispatchMouseDown { get; init; }
        public bool DispatchMouseUp { get; init; }

        public bool DispatchClicks { get; init; }
        public bool DispatchDoubleClicks { get; init; }
    }

    public abstract class MouseBehavior<T> : ITvBehavior<T>
    {

        private readonly MouseBehaviorOptions _options;
        public MouseBehavior(MouseBehaviorOptions options)
        {
            _options = options;
        }
        public bool Update(BehaviorContext<T> updateContext)
        {
            var events = updateContext.Events.MouseEvents;

            var updated = false;

            foreach (var evt in events)
            {
                if (_options.DispatchClicks && evt.IsClickEvent)
                {
                    updated = OnMouseClick(evt, updateContext) || updated;
                }

                if (_options.DispatchDoubleClicks && evt.IsDoubleClickEvent)
                {
                    updated = OnMouseDoubleClick(evt, updateContext) || updated;
                }

                if (_options.DispatchMouseDown && evt.IsPressedEvent)
                {
                    updated = OnMouseDown(evt, updateContext) || updated;
                }

                if (_options.DispatchMouseUp && evt.IsReleasedEvent)
                {
                    updated = OnMouseUp(evt, updateContext) || updated;
                }

            }

            return updated;
        }

        protected virtual bool OnMouseUp(TvConsoleMouseEvent evt, BehaviorContext<T> updateContext) => false;
        protected virtual bool OnMouseDown(TvConsoleMouseEvent evt, BehaviorContext<T> updateContext) => false;
        protected virtual bool OnMouseClick(TvConsoleMouseEvent evt, BehaviorContext<T> updateContext) => false;
        protected virtual bool OnMouseDoubleClick(TvConsoleMouseEvent evt, BehaviorContext<T> updateContext) => false;
    }
}
