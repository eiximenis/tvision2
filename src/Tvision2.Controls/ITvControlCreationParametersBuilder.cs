﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Controls
{
    public interface ITvControlCreationParametersBuilder<TState>
        where TState: IDirtyObject
    {

        ITvControlCreationParametersBuilder<TState> UseSkin(ISkin skin);
        ITvControlCreationParametersBuilder<TState> ConfigureState(Action<TState> stateConfiguration);
        ITvControlCreationParametersBuilder<TState> UseViewport(IViewport viewport);
        ITvControlCreationParametersBuilder<TState> RequestBounds();
        ITvControlCreationParametersBuilder<TState> UseControlName(string name);
    }

}
