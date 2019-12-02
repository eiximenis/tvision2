using System;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{

    public class TvControlCreationParameters
    {
        public ISkin Skin { get; }
        public bool AutoCreateViewport { get; private set; }

        public string Name { get; }

        public ITvControl Owner { get; }

        public TvPoint Position { get; }

        public IViewport Viewport { get; }

        public TvControlCreationParameters(ISkin skin, IViewport viewport)
        {
            Skin = skin;
            Position = viewport?.Position ?? TvPoint.Zero;
            Viewport = viewport;
            AutoCreateViewport = false;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, string name) : this(skin, viewport)
        {
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, string name, ITvControl owner) : this(skin, viewport, name)
        {
            Owner = owner;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position)
        {
            Skin = skin;
            AutoCreateViewport = true;
            Position = position;
            Viewport = null;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, string name) : this(skin, position)
        {
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, string name, ITvControl owner) : this(skin, position, name)
        {
            Owner = owner;
        }
    }

    public class TvControlCreationParameters<TState> : TvControlCreationParameters
        where TState : IDirtyObject
    {

        public TState InitialState { get; }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState) : base(skin, viewport)
        {
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name) : base(skin, viewport, name)
        {
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name, ITvControl owner) : base(skin, viewport, name, owner)
        {
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState) : base(skin, position)
        {
        
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState, string name) : base(skin, position, name)
        {
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState, string name, ITvControl owner) : base(skin, position, name, owner)
        {
            InitialState = initialState;
        }
    }
}
