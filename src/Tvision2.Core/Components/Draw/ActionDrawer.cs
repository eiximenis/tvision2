using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{
    public class ActionDrawer: ITvDrawer
    {
        private readonly Action<RenderContext> _drawFunc;

        public ActionDrawer(Action<RenderContext> drawFunc)
        {
            _drawFunc = drawFunc;
        }

        public void Draw(RenderContext context) => _drawFunc.Invoke(context);
    }


    public class ActionDrawer<TOptions> : ITvDrawer
    where TOptions : class, new()
    {

        private TOptions _options;
        private readonly Action<RenderContext, TOptions> _drawFunc;

        public ActionDrawer(Action<RenderContext, TOptions> drawFunc, Action<TOptions> optionsAction)
        {
            _options = new TOptions();
            optionsAction?.Invoke(_options);
            _drawFunc = drawFunc;
        }

        public void Draw(RenderContext context) => _drawFunc.Invoke(context, _options);
    }
}
