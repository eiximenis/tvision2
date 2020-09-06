using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;

namespace Tvision2.Styles.Backgrounds
{
    public interface IBackgroundProvider
    {

        bool IsFixedBackgroundColor { get; }
        TvColor GetColorFor(int row, int col, TvBounds bounds);
    }
}
