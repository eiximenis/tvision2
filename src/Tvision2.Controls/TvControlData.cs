using System;
using Tvision2.Core.Styles;

namespace Tvision2.Controls
{

    public class TvControlData : IControlData
    {
        public bool IsDirty { get; protected set; }
        public StyleSheet Style { get; private set; }
        public string Name { get; }
        public void Reset() { IsDirty = false; }

        public TvControlData(StyleSheet style, string name)
        {
            Style = style ?? throw new ArgumentNullException(nameof(style));
            Name = name ?? string.Empty;
        }
    }
}