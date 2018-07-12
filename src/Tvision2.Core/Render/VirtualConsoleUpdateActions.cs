using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class VirtualConsoleUpdateActions
    {

        private readonly List<VirtualConsoleUpdate> _diffs;
        private readonly VirtualConsoleCursor _cursor;
        public VirtualConsoleUpdateActions(List<VirtualConsoleUpdate> diffs, VirtualConsoleCursor cursor)
        {
            _diffs = diffs;
            _cursor = cursor;
        }

        public IEnumerable<VirtualConsoleUpdate> Diffs => _diffs;

        public (bool, TvPoint) CursorMovement => (_cursor.MovementPending, _cursor.Position);


    }
}
