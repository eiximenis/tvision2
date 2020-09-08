using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{
    public struct DrawResult
    {
        private static DrawResult _done = new DrawResult(TvPoint.Zero, TvBounds.FromRowsAndCols(0, 0));
        public static DrawResult Done { get => _done; }
        public TvPoint Displacement { get; }

        public TvBounds BoundsAdjustement { get; }

        public DrawResult(in TvPoint displacement, in TvBounds adjustement)
        {
            Displacement = displacement;
            BoundsAdjustement = adjustement;
        }

        public static bool operator ==(in DrawResult one, in DrawResult two)
        {
            return one.Displacement == two.Displacement && one.BoundsAdjustement == two.BoundsAdjustement;
        }

        public static bool operator !=(in DrawResult one, in DrawResult two) => !(one == two);
    }
}