using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{
    public class ActionDrawer<T>: ITvDrawer<T>
    {
        private readonly Action<RenderContext<T>> _drawFunc;

        public ActionDrawer(Action<RenderContext<T>> drawFunc)
        {
            _drawFunc = drawFunc;
        }

        public DrawResult Draw(RenderContext<T> context)
        {
            _drawFunc.Invoke(context);
            return DrawResult.Done;
        }


        DrawResult ITvDrawer.Draw(RenderContext context)
        {
            return Draw(context as RenderContext<T>);
        }
    }


    public class ActionDrawer<T,TOptions> : ITvDrawer<T>
    where TOptions : class, new()
    {

        private TOptions _options;
        private readonly Action<RenderContext<T>, TOptions> _drawFunc;

        public ActionDrawer(Action<RenderContext<T>, TOptions> drawFunc, Action<TOptions> optionsAction)
        {
            _options = new TOptions();
            optionsAction?.Invoke(_options);
            _drawFunc = drawFunc;
        }

        public DrawResult Draw(RenderContext<T> context)
        {
            _drawFunc.Invoke(context, _options);
            return DrawResult.Done;
        }

        DrawResult ITvDrawer.Draw(RenderContext context)
        {
            return Draw(context as RenderContext<T>);
        }
    }
}
