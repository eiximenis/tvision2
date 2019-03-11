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

        public ISkin SkinToUse { get; private set; }
        public Action<TState> StateConfigurator { get; private set; }
        public IViewport Viewport { get; private set; }
        private readonly Func<TState> _stateCreator;
        public Func<TState, TvPoint, IViewport> ViewportCreator { get; private set; }
        private string _name;

        public TvPoint Position { get; private set; }


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
            if (ViewportCreator != null)
            {
                throw new InvalidOperationException("Can't set Viewport and ViewportBuilder together");
            }
            Viewport = viewport;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> CreateViewportFromState(Func<TState, TvPoint, IViewport> viewportCreator)
        {
            if (Viewport != null)
            {
                throw new InvalidOperationException("Can't set Viewport AND ViewportCreator together");
            }
            ViewportCreator = viewportCreator;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> UseTopLeftPosition(TvPoint position)
        {
            if (Viewport != null)
            {
                throw new InvalidOperationException("Can't set Viewport AND position together");
            }
            Position = position;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> UseControlName(string name)
        {
            _name = name;
            return this;
        }

        public TvControlCreationParameters<TState> Build()
        {
            var state = _stateCreator();
            StateConfigurator?.Invoke(state);
            return Viewport != null
                ? new TvControlCreationParameters<TState>(SkinToUse, Viewport, state, _name)
                : new TvControlCreationParameters<TState>(SkinToUse, ViewportCreator ?? FixedViewportCreator<TState>.NullViewport(),  Position, state, _name);
        }

    }
}
