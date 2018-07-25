using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    public class TvControlMetadata : IDirtyObject
    {
        public ITvControl Control { get; }

        public bool IsFocused { get; private set; }

        public bool IsDirty { get; private set; }
        public bool CanFocus { get; internal set; }

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

        public TvControlMetadata(ITvControl control)
        {
            Control = control;
            IsFocused = false;
            CanFocus = true;
        }
    }
}
