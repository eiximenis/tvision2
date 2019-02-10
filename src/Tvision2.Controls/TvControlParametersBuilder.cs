using System;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public static class TvControlCreationParametersBuilder
    {
        public static ITvControlCreationParametersBuilder<TState> ForState<TState>(Func<TState> stateCreator, Action<TState> stateCfg = null)
            where TState : IDirtyObject
        {
            return new TvControlCreationParametersBuilder<TState>(stateCreator).ConfigureState(stateCfg);
        }
        public static ITvControlCreationParametersBuilder<TState> ForDefaultState<TState>(Action<TState> stateCfg = null)
            where TState : IDirtyObject, new()
        {
            return new TvControlCreationParametersBuilder<TState>(() =>
            {
                var state = new TState();
                return state;
            }).ConfigureState(stateCfg);
        }

    }
    class TvControlCreationParametersBuilder<TState> : ITvControlCreationParametersBuilder<TState>
        where TState : IDirtyObject
    {

        protected ISkin SkinToUse { get; private set; }
        protected Action<TState> StateConfigurator { get; private set; }
        protected IViewport Viewport { get; private set; }
        private readonly Func<TState> _stateCreator;

        public TvControlCreationParametersBuilder(Func<TState> stateCreator)
        {
            _stateCreator = stateCreator;
        }

        public ITvControlCreationParametersBuilder<TState> UseSkin(ISkin skin)
        {
            SkinToUse = skin;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> ConfigureState(Action<TState> stateConfiguration)
        {
            StateConfigurator = stateConfiguration;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> UseViewport(IViewport viewport)
        {
            Viewport = viewport;
            return this;
        }

        public TvControlCreationParameters<TState> Build()
        {
            var state = _stateCreator();
            StateConfigurator?.Invoke(state);
            return new TvControlCreationParameters<TState>(SkinToUse, Viewport, state);
        }
    }
}
