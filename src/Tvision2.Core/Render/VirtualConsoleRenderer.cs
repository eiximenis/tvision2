using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public static class VirtualConsoleRenderer
    {
        public static void RenderToConsole(IEnumerable<VirtualConsoleUpdate> diffs)
        {
            foreach (var diff in diffs)
            {
                RendererManager.DrawAt(diff.New, diff.Column, diff.Row);
            }
        }
    }
}
