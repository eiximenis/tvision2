using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public interface ITvControlCreationParametersBuilder<TState>
        where TState: IDirtyObject
    {

        ITvControlCreationParametersBuilder<TState> UseSkin(ISkin skin);
        ITvControlCreationParametersBuilder<TState> ConfigureState(Action<TState> stateConfiguration);
        ITvControlCreationParametersBuilder<TState> UseViewport(IViewport viewport);
        ITvControlCreationParametersBuilder<TState> UseTopLeftPosition(TvPoint position);
        ITvControlCreationParametersBuilder<TState> CreateViewportFromState(Func<TState, TvPoint, IViewport> viewportCreator);
        ITvControlCreationParametersBuilder<TState> UseControlName(string name);
        TvControlCreationParameters<TState> Build();
    }

}
