using Tvision2.Core.Styles;

namespace Tvision2.Controls
{

    public class TvControlData : IControlData
    {
        public bool IsDirty { get; protected set; }
        public StyleSheet Style { get; private set; }
        public string Name { get; set; }
        public void Reset() { IsDirty = false; }

        public TvControlData()
        {
            Style = new StyleSheet();
            Name = string.Empty;
        }
    }
}