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
        public static ITvControlCreationParametersBuilder<TState> ForState<TState>(TState state, Action<TState> stateCfg = null)
            where TState : IDirtyObject
        {
            return new TvControlCreationParametersBuilder<TState>(state).ConfigureState(stateCfg);
        }
        public static ITvControlCreationParametersBuilder<TState> ForDefaultState<TState>(Action<TState> stateCfg = null)
            where TState : IDirtyObject, new()
        {
            return new TvControlCreationParametersBuilder<TState>(() => new TState()).ConfigureState(stateCfg);
        }
    }
    class TvControlCreationParametersBuilder<TState> : ITvControlCreationParametersBuilder<TState>
        where TState : IDirtyObject
    {

        private bool _positionSet;
        public ISkin SkinToUse { get; private set; }
        public Action<TState> StateConfigurator { get; private set; }
        public IViewport Viewport { get; private set; }
        private readonly Func<TState> _stateCreator;
        private string _name;
        private Guid _parentId;
        private readonly TState _initialState;

        public TvPoint Position { get; private set; }
        public TvControlCreationParametersBuilder(TState initialState)
        {
            _stateCreator = null;
            _positionSet = false;
            _parentId = Guid.Empty;
            _initialState = initialState;
        }

        public TvControlCreationParametersBuilder(Func<TState> stateCreator)
        {
            _stateCreator = stateCreator;
            _positionSet = false;
            _parentId = Guid.Empty;
            _initialState = default(TState);
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
            if (_positionSet)
            {
                throw new InvalidOperationException("Can't set Viewport AND position together");
            }
            Viewport = viewport;
            return this;
        }


        public ITvControlCreationParametersBuilder<TState> UseTopLeftPosition(TvPoint position)
        {
            if (Viewport != null)
            {
                throw new InvalidOperationException("Can't set Viewport AND position together");
            }
            Position = position;
            _positionSet = true;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> UseControlName(string name)
        {
            _name = name;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> ChildOf(Guid parentId)
        {
            _parentId = parentId;
            return this;
        }

        public TvControlCreationParameters<TState> Build()
        {
            var state = _stateCreator != null ? _stateCreator() : _initialState;
            StateConfigurator?.Invoke(state);
            return _positionSet
                ? new TvControlCreationParameters<TState>(SkinToUse, Position, state, name: _name)
                : new TvControlCreationParameters<TState>(SkinToUse, Viewport, state, name: _name);
        }

    }
}
