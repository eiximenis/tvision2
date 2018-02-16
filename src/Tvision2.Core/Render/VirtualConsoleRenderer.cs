using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;

namespace Tvision2.Core.Render
{
    public static class VirtualConsoleRenderer
    {
        public static void RenderToConsole(IConsoleDriver console, IEnumerable<VirtualConsoleUpdate> diffs)
        {
            foreach (var diff in diffs)
            {
                RendererManager.DrawAt(console, diff.New, diff.Column, diff.Row);
            }
        }
    }
}
