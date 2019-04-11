using System;
using Tvision2.Core.Colors;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;

namespace Tvision2.Debug
{
    internal class DebugDrawer : ITvDrawer
    {
        private readonly CharacterAttribute _attribute;

        public DebugDrawer()
        {
            _attribute = new CharacterAttribute(new Random().Next(1, 16), CharacterAttributeModifiers.Normal);
        }

        public void Draw(RenderContext context)
        {
            context.Fill(_attribute);
        }
    }
}