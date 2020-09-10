using System;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.ActivityIndicator
{

    class TvActivityIndicatorParamsBuilder : TvControlCreationBuilder<TvActivityIndicator, ActivityIndicatorState> { }
    public class TvActivityIndicator : TvControl<ActivityIndicatorState>
    {
        private static readonly char[] _chars = new[] { '|', '/', '-', '\\' };
        private int _ticks;
        private int _idx;

        public static ITvControlOptionsBuilder<TvActivityIndicator, ActivityIndicatorState> UseParams() => new TvActivityIndicatorParamsBuilder();

        public TvActivityIndicator(TvControlCreationParameters<ActivityIndicatorState> parameters) : base(parameters)
        {
            _ticks = 0;
            _idx = 0;
        }

        protected override void AddCustomElements(TvComponent<ActivityIndicatorState> component)
        {
            component.AddBehavior(c =>
            {
                _ticks++;
                if (_ticks == 30)
                {
                    _ticks = 0;
                    _idx++;
                    _idx = _idx % 4;
                    return true;
                }
                return false;
            });
        }

        protected override void OnDraw(RenderContext<ActivityIndicatorState> context)
        {
            var style = this.CurrentStyle.Standard;
            context.DrawChars(_chars[_idx], 1, TvPoint.Zero, style.ToCharacterAttribute(TvPoint.Zero, context.Viewport.Bounds));
        }
    }
}
