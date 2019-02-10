using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public class TvControlCreationParameters<TState>
        where TState : IDirtyObject
    {

        public ISkin Skin { get; }
        public IViewport Viewport { get; }
        public TState InitialState { get; }

        public string Name { get; }

        public ITvControl Owner { get; }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState)
        {
            Skin = skin;
            Viewport = viewport;
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name) : this(skin, viewport, initialState)
        {
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name, ITvControl owner) : this(skin, viewport, initialState, name)
        {
            Owner = owner;
        }

    }
}
