﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Styles;

namespace Tvision2.Controls
{
    public interface IControlState
    {
        bool IsDirty {get;}
        string Name { get; }
        void Reset();
        IPropertyBag GetNewProperties(IPropertyBag oldProps);

        StyleSheet Style { get; }
    }
}
