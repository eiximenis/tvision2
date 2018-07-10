using System;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;

namespace Tvision2.Debug
{
    internal class DebugDrawer : ITvDrawer
    {
        private readonly ConsoleColor _color;

        public DebugDrawer()
        {
            _color = (ConsoleColor)(new Random().Next(1, 16));
        }

        public void Draw(RenderContext context)
        {
            context.Fill(_color);
        }
    }
}