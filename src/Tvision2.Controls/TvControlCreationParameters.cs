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

        public TvPoint Position { get; }

        public IViewport Viewport { get; }

        public Guid ParentId {get; }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, string name = null, Guid? parentId = null)
        {
            Skin = skin;
            Position = viewport?.Position ?? TvPoint.Zero;
            Viewport = viewport;
            AutoCreateViewport = false;
            ParentId = parentId.HasValue ? parentId.Value : Guid.Empty;
            Name = name;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, string name = null, Guid? parentId = null)
        {
            Skin = skin;
            AutoCreateViewport = true;
            Position = position;
            Viewport = null;
            Name = name;
            ParentId = parentId.HasValue ? parentId.Value : Guid.Empty;
        }

    }

    public class TvControlCreationParameters<TState> : TvControlCreationParameters
        where TState : IDirtyObject
    {

        public TState InitialState { get; }

        public TvControlCreationParameters(ISkin skin, IViewport viewport, TState initialState, string name = null,
            Guid? parentId = null) : base(skin, viewport, name, parentId)
        {
            InitialState = initialState;
        }

        public TvControlCreationParameters(ISkin skin, TvPoint position, TState initialState, string name = null,
            Guid? parentId = null) : base(skin, position, name, parentId)
        {
            InitialState = initialState;
        }
    }
}
