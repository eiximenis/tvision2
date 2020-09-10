using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{
    public class MapperDrawer<T, U> : ITvDrawer<T>
    {
        private readonly Func<T, U> _mapper;
        private readonly ITvDrawer<U> _innerDrawer;
        public MapperDrawer(ITvDrawer<U> innerDrawer, Func<T,U> mapper)
        {
            _mapper = mapper;
            _innerDrawer = innerDrawer;
        }
        public DrawResult Draw(RenderContext<T> context)
        {
            return _innerDrawer.Draw(context.AsRenderContext(_mapper));
        }

        DrawResult ITvDrawer.Draw(RenderContext context) => Draw((RenderContext<T>)context);
    }
}
