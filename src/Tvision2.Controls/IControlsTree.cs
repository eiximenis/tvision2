using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls
{
    public interface IControlsTree
    {
        public enum ChildRelationship
        {
            Direct,
            All
        }
        TvControlMetadata NextControl(TvControlMetadata current);
        TvControlMetadata PreviousControl(TvControlMetadata current);
        TvControlMetadata CurrentFocused();
        bool Focus(TvControlMetadata controlToFocus);
        TvControlMetadata First();
        bool MoveFocusToNext();
        IEnumerable<TvControlMetadata> ControlsMetadata { get; }

        IEnumerable<TvControlMetadata> Descendants(TvControlMetadata cdata);
        IEnumerable<TvControlMetadata> Childs(TvControlMetadata cdata);

        TvControlMetadata this[Guid id] { get; }

        bool ReturnFocusToPrevious();


        event EventHandler<FocusChangedEventArgs> FocusChanged;
    }
}
