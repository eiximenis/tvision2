namespace Tvision2.Controls.Menu
{

    public interface ITvMenuBarOptions
    {
        ITvMenuBarOptions ItemsSpacedBy(int charcount);
    }

    class TvMenuBarOptions : ITvMenuBarOptions
    {
        public int SpaceBetweenItems { get; private set; }

        public TvMenuBarOptions()
        {
            SpaceBetweenItems = 4;
        }

        ITvMenuBarOptions ITvMenuBarOptions.ItemsSpacedBy(int charcount)
        {
            SpaceBetweenItems = charcount;
            return this;
        }
    }
}