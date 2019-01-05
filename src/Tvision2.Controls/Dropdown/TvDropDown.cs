using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.List;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Dropdown
{
    public class TvDropdown : TvControl<DropdownState>
    {
        private readonly TvList<DropDownValue> _list;
        private readonly IComponentTree _rootUi;

        public TvDropdown(ISkin skin, IViewport boxModel, DropdownState initialState, ITuiEngine engine) : base(skin, boxModel, initialState)
        {
            _list = new TvList<DropDownValue>(skin, boxModel, new ListState<DropDownValue>(initialState.Values));
            _rootUi = engine.UI;
        }

        public static TvDropdown Create(ISkin skin, IViewport viewport, ITuiEngine engine, Action<IBuildableDropDownState> initialStateCreator = null)
        {
            var state = new DropdownState();
            initialStateCreator?.Invoke(state);
            return new TvDropdown(skin, viewport, state, engine);
        }

        internal void DisplayList()
        {
            _rootUi.Remove(this);
            _rootUi.Add(_list);

            // TODO: Need to insert _list in same index as this and make it focused.
            // TODO 2: Maybe a disable/enable option woild be good instead of removing/adding control each time...
        }

        internal void HideList()
        {
            _rootUi.Add(this);
            _rootUi.Remove(_list);
        }

        protected override IEnumerable<ITvBehavior<DropdownState>> GetEventedBehaviors()
        {
            yield return new DropDownBehavior(this);
        }

        public override void OnLostFocus()
        {
            HideList();
        }

        protected override void OnDraw(RenderContext<DropdownState> context)
        {
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
            context.DrawStringAt("value", TvPoint.Zero, pairIdx);
        }
    }
}
