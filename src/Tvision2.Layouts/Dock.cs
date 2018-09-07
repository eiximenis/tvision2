using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Layouts
{
    /// <summary>
    /// Dock determines how to "insert" the viewport of a component inside a container viewport.
    /// 0..5 are exclusive values
    /// FILL is a flag value which can be combined to all others
    /// 
    /// No type is defined (using int32 instead) to allow future containers to use other dock semantics
    /// </summary>
    public static class Dock
    {
        /// <summary>
        /// Item is inserted at the container by just displacing its viewport
        /// If fill, item Fills the whole container
        /// </summary>
        public const int NONE = 0;
        /// <summary>
        /// Item is put on top the container.
        /// If Fill viewport is extended to whole container wide
        /// </summary>
        public const int TOP = 1;
        /// <summary>
        /// Item is put on bottom the container.
        /// If Fill viewport is extended to whole container wide
        /// </summary>
        public const int BOTTOM = 2;
        /// <summary>
        /// Item is put on left of the container.
        /// If Fill viewport is extended to whole container height
        /// </summary>
        public const int LEFT = 3;
        /// <summary>
        /// Item is put on right of the container.
        /// If Fill viewport is extended to whole container height
        /// </summary>
        public const int RIGHT = 4;
        /// <summary>
        /// Item is centered on the container
        /// Fill has no effect
        /// </summary>
        public const int CENTER = 5;
        /// <summary>
        /// Bit-indicator to indicate if fill
        /// </summary>
        public const int FILL = 0x8;

        public static bool HasFill(int dock) => (dock & FILL) == FILL;

        public static int Unfill(int dockMaybeFilled) => dockMaybeFilled & 0x7;
    }


}
