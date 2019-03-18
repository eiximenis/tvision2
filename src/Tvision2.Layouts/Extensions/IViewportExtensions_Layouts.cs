namespace Tvision2.Core.Render
{
    public static class IViewportExtensions_Layouts
    {

        public static IViewport TakeRows(this IViewport viewport, int rowsToTake, int startingRow)
        {
            var availableRows = viewport.Bounds.Rows - startingRow;
            if (rowsToTake > availableRows)
            {
                rowsToTake = availableRows;
            }

            return new Viewport(viewport.Position + new TvPoint(0, startingRow), new TvBounds(rowsToTake, viewport.Bounds.Cols), viewport.ZIndex);
        }

        public static IViewport CreateCentered(this IViewport viewport, int columns, int rows)
        {
            var maxrows = viewport.Bounds.Rows - 2;
            var maxcols = viewport.Bounds.Cols - 2;

            var finalrows = rows < maxrows ? rows : maxrows;
            var finalcols = columns < maxcols ? columns : maxcols;

            return new Viewport(viewport.Position + new TvPoint((maxcols - finalcols) / 2, (maxrows - finalrows) / 2), new TvBounds(finalrows, finalcols), viewport.ZIndex);
        }
    }
}
