using Microsoft.Extensions.Options;
using System;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Controls
{

    abstract class TvControlCreationParametersBuilderBase<TState> : ITvControlCreationParametersBuilder<TState>
        where TState : IDirtyObject
    {
        protected readonly Func<TState> _stateCreator;
        protected string _name;
        protected readonly TState _initialState;
        private IViewport _viewport;
        private bool _requestBounds;

        public ISkin SkinToUse { get; private set; }
        public Action<TState> StateConfigurator { get; private set; }


        public bool BoundsRequested { get => _requestBounds || _viewport == null; }

        public IViewport Viewport
        {
            get => _viewport ?? Tvision2.Core.Render.Viewport.NullViewport;
        }
        public TvPoint Position { get; private set; }

        public TvControlCreationParametersBuilderBase(TState initialState)
        {
            _stateCreator = null;
            _initialState = initialState;
            _viewport = null;
            _requestBounds = false;
        }

        public TvControlCreationParametersBuilderBase(Func<TState> stateCreator)
        {
            _stateCreator = stateCreator;
            _initialState = default(TState);
            _viewport = null;
            _requestBounds = false;
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
            _viewport = viewport;
            return this;
        }


        public ITvControlCreationParametersBuilder<TState> UseControlName(string name)
        {
            _name = name;
            return this;
        }

        public ITvControlCreationParametersBuilder<TState> RequestBounds()
        {
            _requestBounds = true;
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
            return new TvControlCreationParameters<TState, TOptions>(SkinToUse, Viewport, state, _options, mustCreateViewport: BoundsRequested, name: _name);
        }

    }

    class TvControlCreationParametersBuilder<TState> : TvControlCreationParametersBuilderBase<TState>
        where TState : IDirtyObject
    {
        public TvControlCreationParametersBuilder(TState initialState) : base(initialState) { }

        public TvControlCreationParametersBuilder(Func<TState> stateCreator) : base(stateCreator) { }
        public TvControlCreationParameters<TState> Build()
        {
            var state = _stateCreator != null ? _stateCreator() : _initialState;
            StateConfigurator?.Invoke(state);
            return new TvControlCreationParameters<TState>(SkinToUse, Viewport, state, name: _name, mustCreateViewport: BoundsRequested);

        }

    }
}
