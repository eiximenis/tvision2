using Microsoft.Extensions.Options;
using System;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Controls
{

    abstract class TvControlCreationParametersBuilderBase<TState> : ITvControlCreationParametersBuilder<TState>
        where TState : IDirtyObject
    {
        protected bool _positionSet;
        protected readonly Func<TState> _stateCreator;
        protected string _name;
        protected readonly TState _initialState;

        public ISkin SkinToUse { get; private set; }
        public Action<TState> StateConfigurator { get; private set; }
        public IViewport Viewport { get; private set; }
        public TvPoint Position { get; private set; }

        public TvControlCreationParametersBuilderBase(TState initialState)
        {
            _stateCreator = null;
            _positionSet = false;
            _initialState = initialState;
        }

        public TvControlCreationParametersBuilderBase(Func<TState> stateCreator)
        {

            _stateCreator = stateCreator;
            _positionSet = false;
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
    }

    class TvControlCreationParametersBuilder<TState, TOptions> : TvControlCreationParametersBuilderBase<TState>
        where TState : IDirtyObject
    {
        private readonly TOptions _options;
        public TvControlCreationParametersBuilder(TState initialState, TOptions options) : base(initialState)
        {
            _options = options;
        }

        public TvControlCreationParametersBuilder(Func<TState> stateCreator, TOptions options) : base(stateCreator)
        {
            _options = options;
        }

        public TvControlCreationParameters<TState, TOptions> Build()
        {
            var state = _stateCreator != null ? _stateCreator() : _initialState;
            StateConfigurator?.Invoke(state);
            return _positionSet
                ? new TvControlCreationParameters<TState, TOptions>(SkinToUse, Position, state, _options, name: _name)
                : new TvControlCreationParameters<TState, TOptions>(SkinToUse, Viewport, state, _options, name: _name);
        }

    }

    class TvControlCreationParametersBuilder<TState> : TvControlCreationParametersBuilderBase<TState>
        where TState : IDirtyObject
    {
        public TvControlCreationParametersBuilder(TState initialState) : base (initialState) { }

        public TvControlCreationParametersBuilder(Func<TState> stateCreator) : base (stateCreator) { }
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
