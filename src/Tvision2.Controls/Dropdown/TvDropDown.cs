using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tvision2.Controls.Label;
using Tvision2.Controls.List;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Styles;

namespace Tvision2.Controls.Dropdown
{

    public class TvDropDownParamsBuilder : TvControlCreationBuilder<TvDropdown, DropdownState> { }

    public class TvDropdown : TvControl<DropdownState>
    {
        private TvList<DropDownValue> _list;
        private TvLabel _label;
        private IComponentTree _ownerUi;
        private Guid _idClickedSubs;
        private readonly ISkin _skin;

        internal bool HasListDisplayed { get; private set; }
        private bool _hidingList;


        public static ITvControlOptionsBuilder<TvDropdown, DropdownState> UseParams() => new TvDropDownParamsBuilder();

        public TvDropdown(TvControlCreationParameters<DropdownState> parameters) : base(parameters)
        {
            _hidingList = false;
            _skin = parameters.Skin;
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


                var listParams = TvList.UseParams<DropDownValue>()
                    .WithState(initialState)
                    .Configure(c => c
                        .UseViewport(viewport)
                        .UseSkin(_skin)
                        .UseControlName("_list"))
                    .Build();

                _list = new TvList<DropDownValue>(listParams, opt => { });
                _list.Metadata.OnFocusLost.Add(ListLostFocus);
            }

            if (_label == null)
            {
                var labelViewport = new Viewport(viewport.Position, viewport.Bounds.SingleRow(), viewport.ZIndex);
                var labelParameters = TvLabel.UseParams()
                    .WithState(new LabelState() { Text = "value" })
                    .Configure(c => c
                        .UseSkin(_skin)
                        .UseViewport(labelViewport)
                        .UseControlName(Name + "_label"))
                    .Build();

                _label = new TvLabel(labelParameters);
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
            options.IsFocused().WhenItselfOrAnyChildHasFocus();
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
