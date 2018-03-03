using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;

namespace Tvision2.Controls
{
    public class TvControlData
    {
        private AppliedStyle _style;
        private readonly IDirtyObject _state;
        public TvControlData(AppliedStyle style, IDirtyObject state)
        {
            _style = style;
            _state = state;
        }

        public bool IsDirty => _state.IsDirty || _style.IsDirty;

        public void Validate()
        {
            _state.Validate();
            _style.Validate();
        }
    }
}
