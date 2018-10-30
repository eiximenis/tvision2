using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.ActivityIndicator
{
    public class TvActivityIndicator : TvControl<ActivityIndicatorState>
    {
        private static readonly char[] _chars = new[] { '|', '/', '-', '\\' };
        private int _ticks;
        private int _idx;

        public TvActivityIndicator(ISkin skin, IViewport viewport, ActivityIndicatorState state) : base(skin, viewport, state)
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
            var attr = this.CurrentStyle.Standard;
            context.DrawChars(_chars[_idx], 1, TvPoint.Zero, attr);
        }
    }
}
