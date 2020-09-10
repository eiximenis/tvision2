using System;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Controls
{

    public class TvControlCreationParameters
    {
        public ISkin Skin { get; }
        public bool AutoCreateViewport { get; private set; }

        public string Name { get; }

        public TvPoint Position { get; }

        public IViewport Viewport { get; }


        public TvControlCreationParameters(ISkin skin, IViewport viewport, string name = null)
        {
            Skin = skin;
            Position = viewport?.Position ?? TvPoint.Zero;
            Viewport = viewport;
            AutoCreateViewport = false;
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, string name = null)
        {
            Skin = skin;
            AutoCreateViewport = true;
            Position = position;
            Viewport = null;
            Name = name;
        }

    }

    public class TvControlCreationParameters<TState, TOptions> : TvControlCreationParameters
        where TState : IDirtyObject
    {

        public TState InitialState { get; }
        public TOptions Options { get;  }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, TOptions options,  string name = null) : base(skin, viewport, name)
        {
            InitialState = initialState;
            Options = options;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState, TOptions options, string name = null) : base(skin, position, name)
        {
            InitialState = initialState;
            Options = options;
        }

    }

    public class TvControlCreationParameters<TState> : TvControlCreationParameters<TState, EmptyControlOptions>
    where TState : IDirtyObject
    {
        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name = null)
            : base(skin, viewport, initialState, new EmptyControlOptions(), name)
        { }
        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState, string name = null)
            : base(skin, position, initialState, new EmptyControlOptions(), name)
        { }
    }
}
