using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Styles;

namespace Tvision2.Controls
{
    public interface IControlData
    {
        bool IsDirty { get; }
        string Name { get; }
        void Reset();

        AppliedStyle Style { get; }
    }
}
