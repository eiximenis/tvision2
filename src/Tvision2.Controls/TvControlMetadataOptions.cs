using System;
using System.Linq;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    public interface ITvControlMetadataWhenFocusedOptions
    {
        void OnlyAnyChildHasFocus();
        void ItselfOrAnyChildHasFocus();
        void Only(Func<bool> hasFocusPredicate);
        void ItselfOr(Func<bool> hasFocusPredicate);
        void Never();
    }

    public class TvControlMetadataOptions : ITvControlMetadataWhenFocusedOptions
    {
        private Func<bool>? _hasFocusCustomPredicate;
        private bool _focusedIfDescendant;

        internal bool HasVirtualFocus(ComponentTreeNode node) => (_focusedIfDescendant && DescendantFocused(node)) || (_hasFocusCustomPredicate?.Invoke() ?? false);

        private bool DescendantFocused(ComponentTreeNode node)
        {
            return node.Descendants()
                .Where(n => n.HasTag<TvControlMetadata>()).Select(n => n.GetTag<TvControlMetadata>())
                .Any(ctl => ctl.IsFocused);
        }

        internal bool CanHaveFocus { get; private set; }


        internal Action OnLostFocusAction { get; private set; }
        internal Action OnFocusAction { get; private set; }

        internal Func<TvControlMetadata, bool> AcceptFocusPredicate { get; private set; }
        internal bool IsDrawable { get; private set; }

        public TvControlMetadataOptions()
        {
            CanHaveFocus = true;
            IsDrawable = true;
            _focusedIfDescendant = false;
            _hasFocusCustomPredicate = null;
        }

        public ITvControlMetadataWhenFocusedOptions FocusedWhen() => this;

        void ITvControlMetadataWhenFocusedOptions.OnlyAnyChildHasFocus()
        {
            CanHaveFocus = false;
            _focusedIfDescendant = true;
        }

        void ITvControlMetadataWhenFocusedOptions.ItselfOrAnyChildHasFocus()
        {
            CanHaveFocus = true;
            _focusedIfDescendant = true;
        }

        void ITvControlMetadataWhenFocusedOptions.Only(Func<bool> hasFocusPredicate)
        {
            CanHaveFocus = false;
            _hasFocusCustomPredicate = hasFocusPredicate;
            _focusedIfDescendant = false;
        }


        void ITvControlMetadataWhenFocusedOptions.ItselfOr(Func<bool> hasFocusPredicate)
        {
            CanHaveFocus = true;
            _hasFocusCustomPredicate = hasFocusPredicate;
            _focusedIfDescendant = false;
        }


        void ITvControlMetadataWhenFocusedOptions.Never()
        {
            _hasFocusCustomPredicate = null;
            CanHaveFocus = false;
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