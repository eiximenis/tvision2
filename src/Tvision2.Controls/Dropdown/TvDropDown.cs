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
        private TvList<DropDownValue> _list;
        private TvLabel _label;
        private IComponentTree _ownerUi;
        private Guid _idClickedSubs;
        private readonly ISkin _skin;

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
            _skin = parameters.Skin;
            Metadata.CanFocus = false;
            HasListDisplayed = false;
        }



        private void ListLostFocus(TvFocusEventData arg)
        {
            HideList(focusToLabel: false);
        }

        protected override void OnViewportCreated(IViewport viewport)
        {
            if (_list == null)
            {
                var initialState = new ListState<DropDownValue>(State.Values, new TvListColumnSpec<DropDownValue>()
                {
                    Transformer = x => x.Text
                });

                var listParams = TvControlCreationParametersBuilder.ForState(initialState)
                    .UseViewport(viewport).UseSkin(_skin)
                    .UseControlName("_list").ChildOf(Metadata.ControlId);
                _list = new TvList<DropDownValue>(listParams, opt => { });
                _list.Metadata.OnFocusLost.Add(ListLostFocus);
            }

            if (_label == null)
            {
                var labelViewport = new Viewport(viewport.Position, viewport.Bounds.SingleRow(), viewport.ZIndex);
                var labelParameters = new TvControlCreationParameters<LabelState>(_skin, labelViewport,
                    new LabelState() { Text = "value" }, Name + "_label", Metadata.ControlId);
                _label = new TvLabel(labelParameters);
                _label.Metadata.CanFocus = true;
            }

        }

        protected override void OnControlMounted(ITuiEngine engine)
        {
            _idClickedSubs = _list.OnItemClicked.Add(el =>
            {
                State.SelectedValue = el.Text;
                _label.State.Text = el.Text;
            });
            _ownerUi = engine.UI;
            _ownerUi.AddAsChild(_label, this);
        }

        protected override ControlCanBeUnmounted OnControlWillbeUnmounted(ITuiEngine ownerEngine)
        {
            _list.OnItemClicked.Remove(_idClickedSubs);
            _ownerUi = null;
            return ControlCanBeUnmounted.Yes;
        }


        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.AvoidDrawControl();
            options.WhenControlIsAskedIfItHasFocus(() => Metadata.FocusTransferred || _label.Metadata.IsFocused || _list.Metadata.IsFocused);
        }

        internal void DisplayList()
        {
            _ownerUi.AddAsChild(_list, this);
            _list.AsComponent()
                .Metadata.OnComponentMounted
                .AddOnce(ctx =>
                {
                    _list.Metadata.Focus(force: true);
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
                        _hidingList = false;
                    });
            }

            _ownerUi.AddAsChild(_label, this);
            _ownerUi.Remove(_list);
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
    }
}
