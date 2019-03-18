using System;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public class TvControlCreationParameters<TState>
        where TState : IDirtyObject
    {

        public ISkin Skin { get; }
        public TState InitialState { get; }
        public bool AutoCreateViewport { get; private set; }

        public string Name { get; }

        public ITvControl Owner { get; }

        public TvPoint Position { get; }

        public IViewport Viewport { get; }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState)
        {
            Skin = skin;
            InitialState = initialState;
            Position = viewport?.Position ?? TvPoint.Zero;
            Viewport = viewport;
            AutoCreateViewport = false;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name) : this(skin, viewport, initialState)
        {
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name, ITvControl owner) : this(skin, viewport, initialState, name)
        {
            Owner = owner;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState)
        {
            Skin = skin;
            AutoCreateViewport = true;
            Position = position;
            Viewport = null;
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState, string name) : this(skin, position, initialState)
        {
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState, string name, ITvControl owner) : this(skin, position, initialState, name)
        {
            Owner = owner;
        }
    }
}
