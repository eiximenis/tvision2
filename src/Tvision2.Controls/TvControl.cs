using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public string ControlType { get; }
        public IStyle CurrentStyle { get; }
        private TvComponent<TState> _component;
        public TvControlMetadata Metadata => _metadata.Value;
        public TState State { get; }
        public string Name => AsComponent().Name;

        public enum ControlCanBeUnmounted
        {
            Yes = 0,
            No = 1
        }


        public TvControl(TvControlCreationParameters<TState> creationParams)
        {

            var initialState = creationParams.InitialState;
            var name = creationParams.Name;
            _component = new TvComponent<TState>(initialState, name ?? $"TvControl_<$>",
                cfg =>
                {
                    cfg.WhenComponentMounted(ctx => OnComponentMounted(ctx));
                    cfg.WhenComponentUnmounted(ctx => OnComponentUnmounted(ctx));
                    cfg.WhenComponentWillbeUnmounted(ctx => OnComponentWillbeUnmounted(ctx));
                });

            _metadata = new Lazy<TvControlMetadata>(() => new TvControlMetadata(this, creationParams, ConfigureMetadataOptions));
            var typename = GetType().Name.ToLowerInvariant();
            var genericIdx = typename.IndexOf('`');
            ControlType = genericIdx != -1 ? typename.Substring(0, genericIdx) : typename;
            CurrentStyle = creationParams.Skin.GetControlStyle(this);
            State = initialState;


            if (creationParams.AutoCreateViewport)
            {
                Metadata.ViewportAutoCreated = true;
            }
            else
            {
                _component.AddViewport(creationParams.Viewport);
                Metadata.ViewportAutoCreated = false;
            }

        }

        protected virtual void ConfigureMetadataOptions(TvControlMetadataOptions options) { }

        private Task<bool> OnComponentUnmounted(ComponentMoutingContext ctx)
        {
            AsComponent().RemoveAllBehaviors();
            AsComponent().RemoveAllDrawers();
            Metadata.Dettach();
            OnControlUnmounted(ctx.OwnerEngine);
            return Task.FromResult(true);
        }

        protected virtual void OnControlUnmounted(ITuiEngine engine)
        {
        }

        private Task<bool> OnComponentMounted(ComponentMoutingContext ctx)
        {
            if (Metadata.ViewportAutoCreated)
            {
                CreateAndSetViewport();
            }
            OnViewportCreated(AsComponent().Viewport);

            var ctree = ctx.OwnerEngine.GetControlsTree() as ControlsTree;
            ctx.Node.SetTag<TvControlMetadata>(Metadata);
            Metadata.Attach(ctree, ctx.Node);
            AddElements();
            OnControlMounted(ctx.OwnerEngine);
            return Task.FromResult(true);
        }

        protected virtual void OnControlMounted(ITuiEngine owner)
        {
        }

        private void OnComponentWillbeUnmounted(ComponentMountingCancellableContext ctx)
        {
            var canBeCancelled = OnControlWillbeUnmounted(ctx.OwnerEngine);
            if (canBeCancelled == ControlCanBeUnmounted.No)
            {
                ctx.Cancel();
            }
        }

        protected virtual ControlCanBeUnmounted OnControlWillbeUnmounted(ITuiEngine ownerEngine) => ControlCanBeUnmounted.Yes;
        private void CreateAndSetViewport()
        {
            _component.AddViewport(CalculateViewport());
        }

        protected virtual void OnViewportCreated(IViewport viewport) { }

        protected virtual IViewport CalculateViewport()
        {
            throw new InvalidOperationException($"This control (type {GetType().Name} do not support autocreating viewports.");
        }


        private void AddElements()
        {
            _component.AddBehavior(new ControlStateBehavior<TState>(Metadata));
            if (Metadata.IsDrawable)
            {
                _component.AddDrawer(OnDraw);
            }
            foreach (var behavior in GetEventedBehaviors())
            {
                _component.AddBehavior(new FocusControlBeheavior<TState>(Metadata, behavior), opt => opt.UseScheduler(BehaviorSchedule.OnEvents));
                AddOwnerIfRequested(behavior);
            }

            AddCustomElements(_component);
        }


        private void AddOwnerIfRequested(ITvBehavior<TState> behavior)
        {
            var behaviorType = behavior.GetType();
            var property = behaviorType.GetProperties().SingleOrDefault(p => p.GetCustomAttribute<OwnerControlAttribute>() != null);
            if (property != null && property.CanWrite)
            {
                var propType = property.PropertyType;
                var myType = this.GetType();
                if (propType.IsAssignableFrom(myType))
                {
                    property.SetValue(behavior, this);
                }
                else
                {
                    throw new InvalidOperationException($"Property {behaviorType.Name}.{property.Name} of type {propType.Name} can't hold a control of type {myType.Name}");
                }
            }
        }

        protected virtual IEnumerable<ITvBehavior<TState>> GetEventedBehaviors()
        {
            return Enumerable.Empty<ITvBehavior<TState>>();
        }

        protected virtual void OnDraw(RenderContext<TState> context) { }

        protected virtual void AddCustomElements(TvComponent<TState> component) { }


        protected void RequestCursorManagement(Action<ICursorContext, TState> cursorAction)
        {
            _component.AddDrawer(new TvControlCursorDrawer<TState>(cursorAction, Metadata));
        }

        public TvComponent<TState> AsComponent() => _component;

        TvComponent ITvControl.AsComponent() => _component;

    }
}
