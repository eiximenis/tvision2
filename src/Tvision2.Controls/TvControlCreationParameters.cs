using System;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Controls
{

    public class TvControlCreationParameters
    {
        public ISkin Skin { get; }
        public bool AutoSetBounds { get; }

        public string Name { get; }

        public IViewport Viewport { get; }


        public TvControlCreationParameters(ISkin skin, IViewport viewport, string name, bool mustCreateViewport)
        {
            Skin = skin;
            Viewport = viewport;
            AutoSetBounds = mustCreateViewport;
            Name = name;
        }


    }

    public class TvControlCreationParameters<TState, TOptions> : TvControlCreationParameters
        where TState : IDirtyObject
    {

        public TState InitialState { get; }
        public TOptions Options { get; }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, TOptions options, string name = null, bool mustCreateViewport = false) : base(skin, viewport, name, mustCreateViewport)
        {
            InitialState = initialState;
            Options = options;
        }

    }

    public class TvControlCreationParameters<TState> : TvControlCreationParameters<TState, EmptyControlOptions>
    where TState : IDirtyObject
    {
        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name = null, bool mustCreateViewport = false)
            : base(skin, viewport, initialState, new EmptyControlOptions(), name, mustCreateViewport)
        { }
    }
}
