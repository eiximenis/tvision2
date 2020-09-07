using Tvision2.Core.Render;
using Tvision2.Styles.Extensions;

namespace Tvision2.Styles
{
    public struct StyledRenderContext<T>
    {
        private readonly RenderContext<T> _ctx;
        private readonly ISkinManager _skinManager;
        private readonly IStyle _defaultStyle;

        internal StyledRenderContext(RenderContext<T> ctx, StyledRenderContextAdatper adapter, string style)
        {
            _ctx = ctx;
            _skinManager = adapter.SkinManager;
            _defaultStyle = _skinManager.CurrentSkin[style];
        }

        public void DrawChars(char value, int count, TvPoint location) =>
            _ctx.DrawChars(value, count, location, _defaultStyle.Standard);

        public void DrawStringAt(string value, TvPoint location) =>
            _ctx.DrawStringAt(value, location, _defaultStyle.Standard);

        public void Fill() => _ctx.Fill(_defaultStyle.Standard);



    }
}