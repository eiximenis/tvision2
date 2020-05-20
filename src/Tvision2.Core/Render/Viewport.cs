namespace Tvision2.Core.Render
{
    public sealed class Viewport : IViewport
    {
        private static Viewport _nullViewport = new Viewport(TvPoint.Zero, TvBounds.Empty, Layer.Bottom);
        public TvPoint Position { get; }
        public TvBounds Bounds { get; }

        public Layer ZIndex { get; }

        public FlowModel Flow { get; }


        public static IViewport NullViewport => _nullViewport;

        public Viewport(TvPoint point, int cols) : this(point, TvBounds.FromRowsAndCols(0, cols), Layer.Standard, FlowModel.None)
        {
        }

        public Viewport(TvPoint point, TvBounds bounds, Layer zindex, FlowModel flow = FlowModel.None)
        {
            Position = point;
            Bounds = bounds;
            ZIndex = zindex;
            Flow = flow;
        }




        public IViewport ResizeTo(int cols, int rows) => new Viewport(Position, TvBounds.FromRowsAndCols(rows, cols), ZIndex, Flow);
        public IViewport Grow(int ncols, int nrows) => new Viewport(Position, Bounds.Grow(nrows, ncols), ZIndex, Flow);

        public IViewport MoveTo(TvPoint newPos) => new Viewport(newPos, Bounds, ZIndex, Flow);

        public IViewport Translate(TvPoint translation) => new Viewport(Position + translation, Bounds, ZIndex, Flow);

        public IViewport Clone() => new Viewport(Position, Bounds, ZIndex, Flow);



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
                && ZIndex == other.ZIndex
                && Flow == other.Flow;
        }
    }
}
