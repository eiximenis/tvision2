using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{
    public abstract class ConverterDrawer<T, TD> : ITvDrawer<T>
    {
        private readonly Func<T, TD> _converter;
        public ConverterDrawer(Func<T,TD> converter)
        {
            _converter = converter;
        }

        void ITvDrawer<T>.Draw(RenderContext<T> context)
        {
            var newCtx = context.CloneWithNewState(_converter(context.State));
            Draw(newCtx);
        }

        protected abstract void Draw(RenderContext<TD> newCtx);
    }
}
