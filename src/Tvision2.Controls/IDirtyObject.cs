using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls
{
    public interface IDirtyObject
    {
        bool IsDirty { get; }
        void Validate();
    }
}
