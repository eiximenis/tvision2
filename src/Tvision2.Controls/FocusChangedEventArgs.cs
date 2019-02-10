using System;

namespace Tvision2.Controls
{
    public class FocusChangedEventArgs : EventArgs
    {
        public IControlsTree Tree { get; }
        public TvControlMetadata Previous { get; }
        public TvControlMetadata Current { get; }

        public FocusChangedEventArgs(IControlsTree tree, TvControlMetadata previous, TvControlMetadata current)
        {
            Tree = tree;
            Previous = previous;
            Current = current;
        }
    }
}