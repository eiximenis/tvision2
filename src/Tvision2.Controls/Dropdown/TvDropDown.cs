using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tvision2.Controls.Label;
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
        private readonly TvLabel _label;
        private IComponentTree _ownerUi;
        private Guid _idClickedSubs;

        internal bool HasListDisplayed { get; private set; }
        private bool _hidingList;

        public static ITvControlCreationParametersBuilder<DropdownState> CreationParametersBuilder(Action<DropdownState> stateCfg = null)
        {
            return TvControlCreationParametersBuilder.ForDefaultState<DropdownState>(stateCfg);
        }

        public TvDropdown(ITvControlCreationParametersBuilder<DropdownState> parameters) : this(parameters.Build()) { }
        public TvDropdown(TvControlCreationParameters<DropdownState> parameters) : base(parameters)
        {
            _hidingList = false;
            var viewport = parameters.Viewport;
            var labelViewport = new Viewport(viewport.Position, viewport.Columns, 1, viewport.ZIndex);
            var listParameters = new TvControlCreationParameters<ListState<DropDownValue>>(parameters.Skin, viewport, 
                new ListState<DropDownValue>(parameters.InitialState.Values, new TvListColumnSpec<DropDownValue>() { Transformer = x => x.Text }), "_list", this);
            _list = new TvList<DropDownValue>(listParameters, opt =>
            {
                
            });
            var labelParameters = new TvControlCreationParameters<LabelState>(parameters.Skin, labelViewport, new LabelState() { Text = "value" }, Name + "_label", this);
            _label = new TvLabel(labelParameters);
            _label.Metadata.CanFocus = true;
            _list.Metadata.OnFocusLost.Add(ListLostFocus);
            Metadata.CanFocus = false;
            HasListDisplayed = false;
        }



        private Task<bool> ListLostFocus(TvFocusEventData arg)
        {
            HideList(focusToLabel: false);
            return Task.FromResult(true);
        }


        protected override void OnControlMounted(IComponentTree owner)
        {
            _idClickedSubs = _list.OnItemClicked.Add(async el =>
            {
                State.SelectedValue = el.Text;
                _label.State.Text = el.Text;
                return true;
            });
            _ownerUi = owner;
            _ownerUi.Add(_label);
        }

        protected override void OnControlUnmounted(IComponentTree owner)
        {
            _ownerUi.Remove(_label);
            _ownerUi.Remove(_list);
            _list.OnItemClicked.Remove(_idClickedSubs);
            _ownerUi = null;
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.WhenControlIsAskedIfItHasFocus(() => Metadata.FocusTransferred || _label.Metadata.IsFocused || _list.Metadata.IsFocused);
        }

        internal void DisplayList()
        {
            _ownerUi.Add(_list);
            _list.AsComponent()
                .Metadata.OnComponentMounted
                .AddOnce(ctx =>
                {
                    _list.Metadata.Focus(force: true);
                    return Task.FromResult(true);
                });
            
            _ownerUi.Remove(_label);
            HasListDisplayed = true;
            // TODO: Need to insert _list in same index as this and make it focused.
            // TODO 2: Maybe a disable/enable option woild be good instead of removing/adding control each time...

        }

        internal void HideList(bool focusToLabel)
        {
            if (_hidingList) return;
            _hidingList = true;
            if (focusToLabel)
            {
                _label.AsComponent()
                    .Metadata.OnComponentMounted
                    .AddOnce(ctx =>
                    {
                        _label.Metadata.Focus(force: true);
                        return Task.FromResult(true);
                    });
            }

            _ownerUi.Add(_label);
            _ownerUi.Remove(_list);
            _hidingList = false;
            HasListDisplayed = false;
        }

        protected override IEnumerable<ITvBehavior<DropdownState>> GetEventedBehaviors()
        {
            yield return new DropDownBehavior(this);
        }

        private void DoOnFocus()
        {
            if (HasListDisplayed)
            {
                _list.Metadata.Focus(force: true);
            }
            else
            {
                _label.Metadata.Focus(force: true);
            }
        }

        protected override void OnDraw(RenderContext<DropdownState> context)
        {
        }
    }
}
