using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    public class TvControlMetadata : IDirtyObject
    {
        private bool _oldCanFocus;
        public ITvControl Control { get; }

        public Guid ControlId { get; }

        public bool IsFocused { get; private set; }

        public bool IsDirty { get; private set; }
        public bool CanFocus { get; set; }

        public void DisableFocusability()
        {
            _oldCanFocus = CanFocus;
            CanFocus = false;
        }

        public void RestoreFocusability()
        {
            CanFocus = _oldCanFocus;
        }

        public void Focus()
        {
            IsFocused = true;
            IsDirty = true;
            Control.OnFocus();
        }

        public void Unfocus()
        {
            IsDirty = true;
            IsFocused = false;
        }

        public void Validate() => IsDirty = false;

        public TvControlMetadata(ITvControl control, Guid componentId)
        {
            Control = control;
            IsFocused = false;
            CanFocus = true;
            ControlId = componentId;
        }
    }
}
