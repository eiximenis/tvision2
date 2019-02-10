﻿using System;
using Tvision2.Core.Components;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public interface IComponentMetadata
    {
        TvComponent Component { get; }
        event EventHandler<ViewportUpdatedEventArgs> ViewportChanged;
    }
}
