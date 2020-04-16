using System;

namespace Tvision2.Controls
{
    public class TvControlMetadataOptions 
    {
        private Func<bool> _hasFocus;
        internal bool? HasFocus => _hasFocus?.Invoke();

        internal Action OnLostFocusAction { get; private set; }
        internal Action OnFocusAction { get; private set; }

        internal Func<TvControlMetadata, bool> AcceptFocusPredicate { get; private set; }
        internal bool IsDrawable { get; private set; }

        public TvControlMetadataOptions()
        {
            IsDrawable = true;
        }

        public void WhenControlIsAskedIfItHasFocus(Func<bool> hasFocus)
        {
            _hasFocus = hasFocus;
        }

        public void WhenControlLosesFocus(Action losesFocusAction)
        {
            OnLostFocusAction = losesFocusAction;
        }

        public void WhenControlGainsFocus(Action gainsFocusAction)
        {
            OnFocusAction = gainsFocusAction;
        }

        public void WhenControlIsRequestedToAcceptFocus(Func<TvControlMetadata, bool> acceptFocusPredicate)
        {
            AcceptFocusPredicate = acceptFocusPredicate;
        }

        public void AvoidDrawControl()
        {
            IsDrawable = false;
        }

    }
}