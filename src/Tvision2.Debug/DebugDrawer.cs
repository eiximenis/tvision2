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
            var fore = TvColor.White;
            var back = TvColor.FromRaw(new Random().Next(1, 8));
            _attribute = new CharacterAttribute(new TvColorPair(fore, back), CharacterAttributeModifiers.Normal);
        }

        public void Draw(RenderContext context)
        {
            context.Fill(_attribute);
        }
    }
}