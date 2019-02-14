using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{

    public abstract class TvControl<TState> : ITvControl<TState>
        where TState : IDirtyObject
    {
        private readonly Lazy<TvControlMetadata> _metadata;
        private readonly ITvControl _owner;
        public string ControlType { get; }
        public IStyle CurrentStyle { get; }
        private TvComponent<TState> _component;
        public TvControlMetadata Metadata => _metadata.Value;
        public TState State { get; }

        public IViewport Viewport => _component.Viewport;

        public string Name => AsComponent().Name;

        public TvControl(TvControlCreationParameters<TState> creationParams)
        {

            var initialState = creationParams.InitialState;
            var name = creationParams.Name;
            _component = new TvComponent<TState>(initialState, name ?? $"TvControl_<$>",
                cfg =>
                {
                    cfg.WhenComponentMounted(ctx => OnComponentMounted(ctx.OwnerTree));
                    cfg.WhenComponentUnmounted(ctx => OnComponentUnmounted(ctx.OwnerTree));
                });

            _metadata = new Lazy<TvControlMetadata>(() => new TvControlMetadata(this, ConfigureStandardMetadataOptions));
            var typename = GetType().Name.ToLowerInvariant();
            var genericIdx = typename.IndexOf('`');
            ControlType = genericIdx != -1 ? typename.Substring(0, genericIdx) : typename;
            CurrentStyle = creationParams.Skin.GetControlStyle(this);
            State = initialState;
            _component.AddViewport(creationParams.Viewport);
            _owner = creationParams.Owner;
            AddElements();
        }

        private void ConfigureStandardMetadataOptions(TvControlMetadataOptions options)
        {
            if (_owner != null)
            {
                options.UseOwner(_owner);
            }
            ConfigureMetadataOptions(options);
        }

        protected virtual void ConfigureMetadataOptions(TvControlMetadataOptions options) { }

        private Task<bool> OnComponentUnmounted(IComponentTree owner)
        {
            var ctree = _metadata.Value.OwnerTree as ControlsTree;
            ctree.Remove(Metadata);
            OnControlUnmounted(owner);
            _metadata.Value.OwnerTree = null;
            return Task.FromResult(true);
        }

        protected virtual void OnControlUnmounted(IComponentTree owner)
        {
        }

        private Task<bool> OnComponentMounted(IComponentTree owner)
        {
            var ctree = owner.RootControls() as ControlsTree;
            ctree.Add(Metadata);
            OnControlMounted(owner);
            return Task.FromResult(true);
        }

        protected virtual void OnControlMounted(IComponentTree owner)
        {
        }

        private void AddElements()
        {
            _component.AddBehavior(new ControlStateBehavior<TState>(Metadata));
            _component.AddDrawer(OnDraw);
            foreach (var behavior in GetEventedBehaviors())
            {
                _component.AddBehavior(new FocusControlBeheavior<TState>(Metadata, behavior), opt => opt.UseScheduler(BehaviorSchedule.OnEvents));
            }
            AddCustomElements(_component);
        }


        protected virtual IEnumerable<ITvBehavior<TState>> GetEventedBehaviors()
        {
            return Enumerable.Empty<ITvBehavior<TState>>();
        }

        protected abstract void OnDraw(RenderContext<TState> context);

        protected virtual void AddCustomElements(TvComponent<TState> component) { }


        protected void RequestControlManagement(Action<ICursorContext, TState> cursorAction)
        {
            _component.AddDrawer(new TvControlCursorDrawer<TState>(cursorAction, Metadata));
        }

        public TvComponent<TState> AsComponent() => _component;

        TvComponent ITvControl.AsComponent() => _component;
    }
}
