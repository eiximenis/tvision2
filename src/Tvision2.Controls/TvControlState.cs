using Tvision2.Core.Components.Props;
using Tvision2.Core.Styles;

namespace Tvision2.Controls
{
    public class TvControlState : IControlState
    {
        [PropIgnore]
        public bool IsDirty { get; protected set; }
        [PropIgnore]
        public StyleSheet Style {get; private set;}

        public string Name { get; set; }

        public void Reset() { IsDirty = false; }

        public TvControlState()
        {
            Style = new StyleSheet();
            Name = string.Empty;
        }

        public IPropertyBag GetNewProperties(IPropertyBag oldProps)
        {
            var newProps = oldProps.SetValues(this);
            return newProps;
        }
    }
}