﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    public interface IDynamicViewportFactory
    {
        IViewport Create(Func<IViewportFactory, IViewport> creator);
    }
}
