using System;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;

namespace Tvision2.Debug
{
    internal class DebugDrawer : ITvDrawer
    {
        private readonly int _pairIdx;

        public DebugDrawer()
        {
            _pairIdx = new Random().Next(1, 16);
        }

        public void Draw(RenderContext context)
        {
            context.Fill(_pairIdx);
        }
    }
}