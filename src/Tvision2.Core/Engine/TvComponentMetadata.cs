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
        public bool IsResponder { get; internal set; }

        public TvComponentMetadata(TvComponent component)
        {
            Component = component;
            IsResponder = false;
        }
    }

}
