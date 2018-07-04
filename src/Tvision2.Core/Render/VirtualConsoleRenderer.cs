using System.Collections.Generic;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Render
{
    public static class VirtualConsoleRenderer
    {
        public static void RenderToConsole(IConsoleDriver console, IEnumerable<VirtualConsoleUpdate> diffs)
        {
            foreach (var diff in diffs)
            {
                RendererManager.DrawAt(console, diff.NewCharacter, diff.Column, diff.Row);
            }
        }
    }
}
