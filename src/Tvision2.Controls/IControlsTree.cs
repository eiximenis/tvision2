using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls
{
    public interface IControlsTree
    {
        TvControlMetadata NextControl(TvControlMetadata current);
        TvControlMetadata PreviousControl(TvControlMetadata current);
        TvControlMetadata CurrentFocused();
        bool Focus(TvControlMetadata controlToFocus);
        TvControlMetadata First();
        bool MoveFocusToNext();
    }
}
