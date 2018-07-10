using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Layouts
{
    public interface ITvContainer
    {
        TvComponent AsComponent();

        void AddChild(TvComponent component);
    }
}
