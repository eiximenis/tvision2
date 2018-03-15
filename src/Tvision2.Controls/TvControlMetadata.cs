using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    public class TvControlMetadata
    {
        public ITvControl Control { get; }

        public bool IsFocused { get; private set; }

        public void Focus()
        {
            IsFocused = true;
            Control.Style.AddClass("focused");
            Control.OnFocus();
        }

        public void Unfocus()
        {
            IsFocused = false;
            Control.Style.RemoveClass("focused");
        }

        public TvControlMetadata(ITvControl control)
        {
            Control = control;
            IsFocused = false;
        }
    }
}
