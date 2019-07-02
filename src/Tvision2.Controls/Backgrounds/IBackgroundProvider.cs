﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Backgrounds
{
    public interface IBackgroundProvider
    {

        bool IsFixedBackgroundColor { get; }
        TvColor GetColorFor(int row, int col);
    }
}
