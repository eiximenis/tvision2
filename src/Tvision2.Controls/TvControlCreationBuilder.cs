using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Tvision2.Controls
{

    public interface ITvControlOptionsBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions>
        where TControl : TvControl<TState, TIOptions>
        where TState : IDirtyObject, new()
        where TOptions : TIOptions, TOptionsBuilder, new()
    {
        ITvControlOptionsBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> Options(Action<TOptionsBuilder> optionsAction);
        ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithState(TState state);
        ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithState(Func<TState> stateCreator);
        ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithState(Action<TState> stateAction);
        ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithDefaultState();
    }

    public interface ITvControlOptionsBuilder<TControl, TState>
    where TControl : TvControl<TState>
    where TState : IDirtyObject, new()
    {
        ITvControlParamsConfigurerBuilder<TControl, TState> WithState(TState state);
        ITvControlParamsConfigurerBuilder<TControl, TState> WithState(Func<TState> stateCreator);
        ITvControlParamsConfigurerBuilder<TControl, TState> WithState(Action<TState> stateAction);
        ITvControlParamsConfigurerBuilder<TControl, TState> WithDefaultState();
    }

    public interface ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions>
    where TControl : TvControl<TState, TIOptions>
    where TState : IDirtyObject, new()
    where TOptions : TIOptions, TOptionsBuilder, new()
    {
        ITvControlParamsBuilder<TState, TOptionsBuilder, TIOptions> Configure(Action<ITvControlCreationParametersBuilder<TState>> cpbAction);
        TvControlCreationParameters<TState, TIOptions> Build();
    }

    public interface ITvControlParamsConfigurerBuilder<TControl, TState>
    where TControl : TvControl<TState>
    where TState : IDirtyObject, new()
    {
        ITvControlParamsBuilder<TState> Configure(Action<ITvControlCreationParametersBuilder<TState>> cpbAction);
        TvControlCreationParameters<TState> Build();
    }


    public interface ITvControlParamsBuilder<TState, TOptionsBuilder, TIOptions>
        where TState : IDirtyObject, new()
    {
        TvControlCreationParameters<TState, TIOptions> Build();
    }

    public interface ITvControlParamsBuilder<TState>
    where TState : IDirtyObject, new()
    {
        TvControlCreationParameters<TState> Build();
    }




    public class TvControlCreationBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> :
        ITvControlOptionsBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions>,
        ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions>,
        ITvControlParamsBuilder<TState, TOptionsBuilder, TIOptions>
        where TControl : TvControl<TState, TIOptions>
        where TState : IDirtyObject, new()
        where TOptions : TIOptions, TOptionsBuilder, new()
    {
        private readonly TOptions _options;
        private TvControlCreationParametersBuilder<TState, TIOptions> _creationParamsBuilder;

        public TvControlCreationBuilder()
        {
            _options = new TOptions();
        }

        public ITvControlOptionsBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> Options(Action<TOptionsBuilder> optionsAction)
        {
            optionsAction?.Invoke(_options);
            return this;
        }

        public ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithDefaultState() => WithState(new TState());

        public ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithState(TState state)
        {
            _creationParamsBuilder = new TvControlCreationParametersBuilder<TState, TIOptions>(state, _options);
            return this;
        }

        public ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithState(Action<TState> stateAction)
        {
            var state = new TState();
            stateAction?.Invoke(state);
            WithState(state);
            return this;
        }

        public ITvControlParamsConfigurerBuilder<TControl, TState, TOptionsBuilder, TIOptions, TOptions> WithState(Func<TState> stateCreator)
        {
            _creationParamsBuilder = new TvControlCreationParametersBuilder<TState, TIOptions>(stateCreator, _options);
            return this;
        }

        public ITvControlParamsBuilder<TState, TOptionsBuilder, TIOptions> Configure(Action<ITvControlCreationParametersBuilder<TState>> cpbAction)
        {
            cpbAction?.Invoke(_creationParamsBuilder);
            return this;
        }

        public TvControlCreationParameters<TState, TIOptions> Build()
        {
            return _creationParamsBuilder.Build();
        }
    }


    public class TvControlCreationBuilder<TControl, TState> :
    ITvControlOptionsBuilder<TControl, TState>,
    ITvControlParamsConfigurerBuilder<TControl, TState>,
    ITvControlParamsBuilder<TState>
    where TControl : TvControl<TState>
    where TState : IDirtyObject, new()
    {
        private TvControlCreationParametersBuilder<TState> _creationParamsBuilder;


        public ITvControlParamsConfigurerBuilder<TControl, TState> WithDefaultState() => WithState(new TState());

        public ITvControlParamsConfigurerBuilder<TControl, TState> WithState(TState state)
        {
            _creationParamsBuilder = new TvControlCreationParametersBuilder<TState>(state);
            return this;
        }

        public ITvControlParamsConfigurerBuilder<TControl, TState> WithState(Action<TState> stateAction)
        {
            var state = new TState();
            stateAction?.Invoke(state);
            WithState(state);
            return this;
        }

        public ITvControlParamsConfigurerBuilder<TControl, TState> WithState(Func<TState> stateCreator)
        {
            _creationParamsBuilder = new TvControlCreationParametersBuilder<TState>(stateCreator);
            return this;
        }

        public ITvControlParamsBuilder<TState> Configure(Action<ITvControlCreationParametersBuilder<TState>> cpbAction)
        {
            cpbAction?.Invoke(_creationParamsBuilder);
            return this;
        }

        public TvControlCreationParameters<TState> Build()
        {
            return _creationParamsBuilder.Build();
        }
    }
}
