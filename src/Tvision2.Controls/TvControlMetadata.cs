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
        private bool _oldCanFocus;
        public bool FocusTransferred { get; private set; }
        private readonly TvControlMetadataOptions _options;
        public Guid ControlId { get; }

        public IActionChain<TvFocusEventData> OnFocusGained { get; }
        public IActionChain<TvFocusEventData> OnFocusLost { get; }

        public bool IsFocused
        {
            get
            {
                return _options.HasFocus ?? FocusTransferred;
            }
        }

        public bool IsDirty { get; private set; }
        public bool CanFocus { get; set; }
        public IControlsTree OwnerTree { get; internal set; }
        public ITvControl Control { get; }
        public Guid OwnerId { get; private set; }

        public bool AcceptFocus(TvControlMetadata currentFocused)
        {
            var accept = CanFocus;
            if (accept && _options.AcceptFocusPredicate != null)
            {
                accept = _options.AcceptFocusPredicate(currentFocused);
            }

            return accept;
        }


        public TvControlMetadata(ITvControl control, Action<TvControlMetadataOptions> optionsAction = null)
        {
            OnFocusGained = new ActionChain<TvFocusEventData>();
            OnFocusLost = new ActionChain<TvFocusEventData>();
            Control = control;
            FocusTransferred = false;
            CanFocus = true;
            ControlId = control.AsComponent().ComponentId;
            OwnerTree = null;
            _options = new TvControlMetadataOptions();
            optionsAction?.Invoke(_options);
            OwnerId = _options.OwnerId;
        }

        public void DisableFocusability()
        {
            _oldCanFocus = CanFocus;
            CanFocus = false;
        }

        public void RestoreFocusability()
        {
            CanFocus = _oldCanFocus;
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
            OnFocusGained.Invoke(new TvFocusEventData(OwnerTree, ControlId, focusGained: true));
        }

        internal void Unfocus()
        {
            IsDirty = true;
            FocusTransferred = false;
            _options.OnLostFocusAction?.Invoke();
            OnFocusLost.Invoke(new TvFocusEventData(OwnerTree, ControlId, focusGained: false));
        }

        public void Validate() => IsDirty = false;
    }
}
