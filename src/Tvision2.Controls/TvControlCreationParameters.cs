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

        private Func<TState, TvPoint, IViewport> _viewportCreator;

        public string Name { get; }

        public ITvControl Owner { get; }

        public TvPoint Position { get; }

        public IViewport GetViewport(TState state) => _viewportCreator(state, Position);

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState)
        {
            Skin = skin;
            InitialState = initialState;
            Position = viewport.Position;
            _viewportCreator = FixedViewportCreator<TState>.Return(viewport);
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name) : this(skin, viewport, initialState)
        {
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name, ITvControl owner) : this(skin, viewport, initialState, name)
        {
            Owner = owner;
        }

        public TvControlCreationParameters(ISkin skin, Func<TState, TvPoint, IViewport> viewportCreator, TvPoint position, TState initialState)
        {
            Skin = skin;
            _viewportCreator = viewportCreator;
            Position = position;
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, Func<TState, TvPoint, IViewport> viewportCreator, TvPoint position, TState initialState, string name) : this(skin, viewportCreator, position, initialState)
        {
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, Func<TState, TvPoint, IViewport> viewportCreator, TvPoint position, TState initialState, string name, ITvControl owner) : this(skin, viewportCreator, position, initialState, name)
        {
            Owner = owner;
        }
    }
}
