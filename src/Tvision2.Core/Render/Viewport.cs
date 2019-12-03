namespace Tvision2.Core.Render
{
    public sealed class Viewport : IViewport
    {
        private static Viewport _nullViewport = new Viewport(TvPoint.Zero, TvBounds.Empty, Layer.Bottom);
        public TvPoint Position { get; }
        public TvBounds Bounds { get; }

        public Layer ZIndex { get; }


        public static IViewport NullViewport => _nullViewport;

        public Viewport(TvPoint point, int cols) : this(point, TvBounds.FromRowsAndCols(0, cols), Layer.Standard)
        {
        }

        public Viewport(TvPoint point, TvBounds bounds, Layer zindex)
        {
            Position = point;
            Bounds = bounds;
            ZIndex = zindex;
        }




        public IViewport ResizeTo(int cols, int rows) => new Viewport(Position, TvBounds.FromRowsAndCols(rows, cols), ZIndex);
        public IViewport Grow(int ncols, int nrows) => new Viewport(Position, Bounds.Grow(nrows, ncols), ZIndex);

        public IViewport MoveTo(TvPoint newPos) => new Viewport(newPos, Bounds, ZIndex);

        public IViewport Translate(TvPoint translation) => new Viewport(Position + translation, Bounds, ZIndex);

        public IViewport Clone() => new Viewport(Position, Bounds, ZIndex);



        public override bool Equals(object obj)
        {
            if (obj is IViewport)
            {
                return Equals((IViewport)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals(IViewport other)
        {
            return Position == other.Position
                && Bounds == other.Bounds
                && ZIndex == other.ZIndex;
        }
    }
}
