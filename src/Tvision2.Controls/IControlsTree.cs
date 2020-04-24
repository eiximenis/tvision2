﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls
{
    public interface IControlsTree
    {
        TvControlMetadata NextControlForFocus(TvControlMetadata current);
        TvControlMetadata PreviousControlForFocus(TvControlMetadata current);
        TvControlMetadata CurrentFocused();
        bool Focus(TvControlMetadata controlToFocus);
        TvControlMetadata First();
        bool MoveFocusToNext();
        IEnumerable<TvControlMetadata> ControlsMetadata { get; }

        TvControlMetadata this[Guid id] { get; }

        bool ReturnFocusToPrevious();

        void CaptureFocus(TvControlMetadata control);
        void FreeFocus();


        event EventHandler<FocusChangedEventArgs> FocusChanged;
    }
}
