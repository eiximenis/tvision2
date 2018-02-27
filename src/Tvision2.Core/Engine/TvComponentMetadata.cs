using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Core.Engine
{
    public class TvComponentMetadata
    {
        public TvComponent Component { get; }
        public Viewport Viewport { get; }

        public TvComponentMetadata(TvComponent component, Viewport viewport)
        {
            Component = component;
            Viewport = viewport;
        }
    }

}
