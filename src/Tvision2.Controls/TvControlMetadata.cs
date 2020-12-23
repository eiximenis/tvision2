using System;
using System.Threading;
using Tvision2.Core;
using Tvision2.Core.Engine;

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
        private IControlsTree _ownerTree;
        public Guid ControlId { get; }

        private readonly ActionChain<TvFocusEventData> _onFocusGained;
        private readonly ActionChain<TvFocusEventData> _onFocusLost;

        public IActionChain<TvFocusEventData> OnFocusGained => _onFocusGained;
        public IActionChain<TvFocusEventData> OnFocusLost => _onFocusLost;

        public void ReturnFocusToPrevious()
        {
            _ownerTree.ReturnFocusToPrevious();
        }

        public bool IsFocused
        {
            get
            {
                return _options.HasVirtualFocus(ComponentNode) || FocusTransferred;
            }
        }

        public bool IsDrawable => _options.IsDrawable;

        public bool IsDirty { get; private set; }
        public bool CanFocus { get; private set; }
        public bool IsAttached { get => _ownerTree != null; }
        public ITvControl Control { get; }

        public ComponentTreeNode ComponentNode { get; private set; }

        public TvControlCreationParameters CreationParameters { get; }

        internal bool AcceptFocus(TvControlMetadata currentFocused)
        {
            var accept = CanFocus;
            if (accept && _options.AcceptFocusPredicate != null)
            {
                accept = _options.AcceptFocusPredicate(currentFocused);
            }

            return accept;
        }

        public void CaptureFocus()
        {
            if (IsAttached)
            {
                _ownerTree.CaptureFocus(this);
            }
        }

        public void ReleaseFocus()
        {
            if (IsAttached)
            {
                _ownerTree.ReleaseFocus();
            }
        }


        public TvControlMetadata(ITvControl control, TvControlCreationParameters creationParameters, Action<TvControlMetadataOptions> optionsAction = null)
        {
            ComponentNode = null;
            CreationParameters = creationParameters;
            _onFocusGained = new ActionChain<TvFocusEventData>();
            _onFocusLost = new ActionChain<TvFocusEventData>();
            Control = control;
            FocusTransferred = false;
            CanFocus = false;
            ControlId = control.AsComponent().ComponentId;
            _ownerTree = null;
            _options = new TvControlMetadataOptions();
            optionsAction?.Invoke(_options);
        }

        internal void Dettach()
        {
            CanFocus = false;
            _ownerTree = null;
            ComponentNode = null;
        }

        internal void Attach(ControlsTree controlsTree, ComponentTreeNode node)
        {
            CanFocus = _options.CanHaveFocus;
            _ownerTree = controlsTree;
            ComponentNode = node;
        }

        public void Focus(bool force = false)
        {
            if (IsAttached && (force || CanFocus))
            {
                _ownerTree.Focus(this);
            }
        }

        internal void DoFocus()
        {
            FocusTransferred = true;
            IsDirty = true;
            _options.OnFocusAction?.Invoke();
            _onFocusGained.Invoke(new TvFocusEventData(_ownerTree, ControlId, focusGained: true));
        }

        internal void Unfocus()
        {
            IsDirty = true;
            FocusTransferred = false;
            _options.OnLostFocusAction?.Invoke();
            _onFocusLost.Invoke(new TvFocusEventData(_ownerTree, ControlId, focusGained: false));
        }
        public void Validate() => IsDirty = false;
    }
}
