using System;
using Tvision2.Core;

namespace Tvision2.Controls
{


    public class TvFocusEventData
    {
        public bool FocusGained { get; }
        public IControlsTree ControlsTree { get; }
        public Guid ControlId { get; }

        public TvFocusEventData(IControlsTree controlsTree, Guid controlId, bool focusGained)
        {
            ControlsTree = controlsTree;
            ControlId = controlId;
            focusGained = FocusGained;
        }
    }


    public class TvControlMetadata : IDirtyObject
    {
        public bool FocusTransferred { get; private set; }
        private readonly TvControlMetadataOptions _options;
        public Guid ControlId { get; }

        private readonly ActionChain<TvFocusEventData> _onFocusGained;
        private readonly ActionChain<TvFocusEventData> _onFocusLost;

        public IActionChain<TvFocusEventData> OnFocusGained => _onFocusGained;
        public IActionChain<TvFocusEventData> OnFocusLost => _onFocusLost;

        public bool IsFocused
        {
            get
            {
                return _options.HasFocus ?? FocusTransferred;
            }
        }

        public bool ViewportAutoCreated { get; internal set; }

        public bool IsDrawable => _options.IsDrawable;

        public bool IsDirty { get; private set; }
        public bool CanFocus { get; set; }
        public IControlsTree OwnerTree { get; internal set; }
        public bool IsAttached { get => OwnerTree != null; }
        public ITvControl Control { get; }
        public Guid ParentId { get; private set; }



        public TvControlCreationParameters CreationParameters { get; }

        public bool AcceptFocus(TvControlMetadata currentFocused)
        {
            var accept = CanFocus;
            if (accept && _options.AcceptFocusPredicate != null)
            {
                accept = _options.AcceptFocusPredicate(currentFocused);
            }

            return accept;
        }


        public TvControlMetadata(ITvControl control, TvControlCreationParameters creationParameters, Action<TvControlMetadataOptions> optionsAction = null)
        {
            CreationParameters = creationParameters;
            _onFocusGained = new ActionChain<TvFocusEventData>();
            _onFocusLost = new ActionChain<TvFocusEventData>();
            Control = control;
            FocusTransferred = false;
            CanFocus = true;
            ControlId = control.AsComponent().ComponentId;
            OwnerTree = null;
            ParentId = creationParameters.ParentId;
            _options = new TvControlMetadataOptions();
            optionsAction?.Invoke(_options);
        }

        public void Focus(bool force = false)
        {
            if (force || CanFocus)
            {
                OwnerTree?.Focus(this);
            }
        }

        internal void DoFocus()
        {
            FocusTransferred = true;
            IsDirty = true;
            _options.OnFocusAction?.Invoke();
            _onFocusGained.Invoke(new TvFocusEventData(OwnerTree, ControlId, focusGained: true));
        }

        internal void Unfocus()
        {
            IsDirty = true;
            FocusTransferred = false;
            _options.OnLostFocusAction?.Invoke();
            _onFocusLost.Invoke(new TvFocusEventData(OwnerTree, ControlId, focusGained: false));
        }
        public void Validate() => IsDirty = false;


        public void CaptureControl(TvControlMetadata control)
        { 
            if (!control.IsAttached)
            {
                control.ParentId = this.ControlId;
                return;
            }
            // TODO: Need a better way. Do we need to expose full ControlsTree as IControlsTree? Drawbacks?
            //((ControlsTree)OwnerTree).Adopt(this, control);
            control.ParentId = this.ControlId;
        }
    }
}
