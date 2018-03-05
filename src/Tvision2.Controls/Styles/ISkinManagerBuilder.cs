﻿using System;

namespace Tvision2.Controls.Styles
{
    public interface ISkinManagerBuilder
    {
        ISkinManagerBuilder AddSkin(string name, ISkin skin);
        ISkinManagerBuilder AddSkin(string name, Action<ISkinBuilder> builderOptions);
    }
}