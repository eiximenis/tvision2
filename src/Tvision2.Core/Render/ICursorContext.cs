using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public interface ICursorContext
    {
        void SetCursorAt(int left, int top);
        void HideCursor();
    }
}
